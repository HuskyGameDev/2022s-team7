using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pegasus : MonoBehaviour
{
    // General Variables
    Rigidbody2D rb;
    [SerializeField] float radius; // The length away from the player to aim for
    bool withinRadius; // Keeps track of if the pegasus is within the desired radius of the player
    float distance; // The distance from the player
    float vX; // X velocity to target
    float vY; // Y velocity to target
    Vector2 playerPos; // Keeps track of the player position
    [SerializeField] float speed; // The (maximum) speeed at which to move
    [SerializeField] int health; // hits required to kill the pegasus
    float invincibility = 2; // Seconds of invincibility after each hit
    float invincibilityTimer;
    float accelerationRate = 0.06f;
    float attackAccelerationRate = 0.30f;

    // Attack Variables
    [SerializeField] float attackSpeed; // The movement speed while attack
    [SerializeField] float attackCooldown; // The time between each attack
    float attackTimer; // Timer to keep track of the cooldown
    float attackLength; // Length in seconds of each attack
    bool attacking; // Boolean to keep track of whether or not the pegasus is currently performing an attack
    Vector2 attackTarget; // The coordiates of the target to attack
    [SerializeField] float attackDelay = 1; // Time (in seconds) to pause before attacking
    float attackDelayTimer;
    bool delayed;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        withinRadius = false;
        attackTimer = attackCooldown;
        attackLength = (radius * 2) / attackSpeed;
        attacking = false;
        attackTarget = Vector2.zero;
        invincibilityTimer = 0;
        attackDelayTimer = 0;
        delayed = false;
    }

    // I'm pretty much only using this for timers
    // (pro tip: timers that use Time.deltaTime **WILL NOT WORK** if they're in FixedUpdate instead of Update)
    private void Update()
    {
        // Updates the cooldown timer
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if(attackLength > 0)
        {
            attackLength -= Time.deltaTime;
        }

        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }

        if(attackDelayTimer > 0)
        {
            attackDelayTimer -= Time.deltaTime;
            if(attackDelayTimer <= 0)
            {
                delayed = true;
            }
        }
    }

    private void FixedUpdate()
    {
        playerPos = Player.getPosition();
        distance = Mathf.Sqrt(Mathf.Pow(playerPos.x - transform.position.x, 2) + Mathf.Pow(playerPos.y - transform.position.y, 2)); // Distance formula

        // If the pegasus is too far away from the player, move toward the player.
        if(distance > radius && !withinRadius)
        {
            Vector2 ratio = new Vector2((transform.position.x - playerPos.x) / (transform.position.y - playerPos.y), (transform.position.y - playerPos.y) / (transform.position.x - playerPos.x));
            ratio.Normalize();
            vX = playerPos.x < transform.position.x ? -speed * Mathf.Abs(ratio.x) : speed * Mathf.Abs(ratio.x);
            vY = playerPos.y < transform.position.y ? -speed * Mathf.Abs(ratio.y) : speed * Mathf.Abs(ratio.y);
        } else
        {
            withinRadius = true;
        }

        // If the pegasus has gotten separated from the player, retrack them
        if(distance > radius * 1.5 && !attacking && attackDelayTimer <= 0)
        {
            withinRadius = false;
        }

        // If the pegasus is roughly within the radius, move to a spot angled about 45 degrees from the player and above them, while maintaining desired distance from the player.
        if(withinRadius && !attacking && attackDelayTimer <= 0)
        {
            // If on the right side of the player
            if(transform.position.x > playerPos.x)
            {
                // Calculate where to aim for
                Vector2 target = new Vector2(playerPos.x + radius * (1 / Mathf.Sqrt(2)), playerPos.y + radius * (1 / Mathf.Sqrt(2)));
                vX = distance < radius ? speed * 0.5f : -speed * 0.5f;
                vY = transform.position.y < target.y ? speed * 0.5f : -speed * 0.5f;
            }
            // If on the left side of the player
            else
            {
                Vector2 target = new Vector2(playerPos.x - radius * (1 / Mathf.Sqrt(2)), playerPos.y + radius * (1 / Mathf.Sqrt(2)));
                vX = distance > radius ? speed * 0.5f : -speed * 0.5f;
                vY = transform.position.y < target.y ? speed * 0.5f : -speed * 0.5f;
            }
        }

        if(withinRadius && attackTimer <= 0 && !delayed && attackDelayTimer <= 0)
        {
            attackDelayTimer = attackDelay;
            vX = 0;
            vY = 0;
        }

        // If close enough to the player and cooldown timer is complete, attack
        if(delayed)
        {
            // Runs at the beginning of each attack. Sets the angle at which the pegasus will travel.
            if (!attacking)
            {
                float slope = (transform.position.y - playerPos.y) / (transform.position.x - playerPos.x);
                if (transform.position.x < playerPos.x)
                {
                    attackTarget = new Vector2(transform.position.x + 2 * radius * (1 / Mathf.Sqrt(1 + Mathf.Pow(slope, 2))), transform.position.y + 2 * radius * (slope / Mathf.Sqrt(1 + Mathf.Pow(slope, 2))));
                }
                else
                {
                    attackTarget = new Vector2(transform.position.x - 2 * radius * (1 / Mathf.Sqrt(1 + Mathf.Pow(slope, 2))), transform.position.y - 2 * radius * (slope / Mathf.Sqrt(1 + Mathf.Pow(slope, 2))));
                }
                attackLength = (radius * 4) / attackSpeed;
            }

            attacking = true;

            // Runs while the attack timer is running
            if(attackLength > 0)
            {
                vX = transform.position.x < attackTarget.x ? 0.5f * attackSpeed : -0.5f * attackSpeed;
                vY = transform.position.y < attackTarget.y ? 0.5f * attackSpeed : -0.5f * attackSpeed;
            }
            // Ends the attack
            else
            {
                attacking = false;
                attackTimer = attackCooldown;
                withinRadius = false;
                delayed = false;
            }
        }

        if(attacking)
        {
            accelerate(attackAccelerationRate);
        } else
        {
            accelerate(accelerationRate);
        }
    }

    // If the horse collides with a wall, stop attacking/reset attack cooldown
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Layer 3 is the ground layer
        if(collision.gameObject.layer == 3)
        {
            attackTimer = attackCooldown;
            withinRadius = false;
            attacking = false;
            delayed = false;
        }

        // Layer 6 is the weapon layer
        if(collision.gameObject.layer == 6 && invincibilityTimer <= 0 && collision.gameObject.GetComponent<Spear>().getTipCollision(gameObject))
        {
            health -= 1;
            invincibilityTimer = invincibility;

            if(health == 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Accelerates to a velocity. Uses vX and vY as targets.
    private void accelerate(float accel)
    {
        
        float x = rb.velocity.x;
        float y = rb.velocity.y;

        if(rb.velocity.x < vX)
        {
            x = rb.velocity.x + accel;
        } 
        else if(rb.velocity.x > vX)
        {
            x = rb.velocity.x - accel;
        }

        if(rb.velocity.y < vY)
        {
            y = rb.velocity.y + accel;
        } 
        else if (rb.velocity.y > vY)
        {
            y = rb.velocity.y - accel;
        }

        if(Mathf.Abs(vY - rb.velocity.y) < (2 * accel))
        {
            y = vY;
        }
        if (Mathf.Abs(vX - rb.velocity.x) < (2 * accel))
        {
            x = vX;
        }

        rb.velocity = new Vector2(x, y);
    }
}
