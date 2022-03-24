using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crocodile : MonoBehaviour
{
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
	boolean velRSwitch = false;
	boolean velYSwitch = false;
	float initialVel = rb.velocity.x;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	    
	private void FixedUpdate()
    {
	// Retarget each update.
        target = UpdateTarget();
        UpdateVelocity();
		accelerate(accelerationRate);
    }
	
	  private void UpdateVelocity()
    {
        /****** X VELOCITY ******/
        // If the target is to the left
        if (target.x < rb.position.x && (rb.velocity.magnitude < maxVelocity || rb.velocity.x > 0))
        {
			if (velRSwitch){
				rb.velocity.x = initialVel;
			}
            if (rb.velocity.x > minVelocity) // If the velocity is positive, but should be negative
            {
                rb.velocity = new Vector2(rb.velocity.x / adjust, rb.velocity.y);
            }
            else if (Mathf.Abs(rb.velocity.x) <= minVelocity) // If the velocity is too close to zero
            {
                rb.velocity = new Vector2(rb.velocity.x - minVelocity, rb.velocity.y);
            }
            else // If the velocity is negative, but needs to be smaller
            {
                rb.velocity = new Vector2(rb.velocity.x * adjust, rb.velocity.y);
            }
			velRSwitch = true;

        }
        // If the target is to the right
        else if (target.x > rb.position.x && (rb.velocity.magnitude < maxVelocity || rb.velocity.x < 0))
        {
			if (velLSwitch){
				rb.velocity.x = initialVel;
			}
            if (rb.velocity.x > minVelocity) // If the velocity is positive, but needs to be larger
            {
                rb.velocity = new Vector2(rb.velocity.x * adjust, rb.velocity.y);
            }
            else if (Mathf.Abs(rb.velocity.x) <= minVelocity) // If the velocity is too close to zero
            {
                rb.velocity = new Vector2(rb.velocity.x + minVelocity, rb.velocity.y);
            }
            else // If the velocity is negative, but needs to be positive
            {
                rb.velocity = new Vector2(rb.velocity.x / adjust, rb.velocity.y);
            }
			velLSwitch = true;
        }


        /****** Y VELOCITY ******/
		// Need to figure out how to add platform traversal for Y axis. Just going to operate on X-axis for now.
		/*
        if (target.y < rb.position.y && (rb.velocity.magnitude < maxVelocity || rb.velocity.y > 0))
        {
            if (rb.velocity.y > minVelocity) // If the velocity is positive, but should be negative
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / adjust);
            }
            else if (Mathf.Abs(rb.velocity.y) <= minVelocity) // If the velocity is too close to zero
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - minVelocity);
            }
            else // If the velocity is negative, but needs to be smaller
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * adjust);
            }
        }
        else if (target.y > rb.position.y && (rb.velocity.magnitude < maxVelocity || rb.velocity.y < 0))
        {
            if (rb.velocity.y > minVelocity) // If the velocity is positive, but should be larger
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * adjust);
            }
            else if (Mathf.Abs(rb.velocity.y) <= minVelocity) // If the velocity is too close to zero
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + minVelocity);
            }
            else // If the velocity is negative, but needs to be positive
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / adjust);
            }
        }
		*/
    }

    private Vector2 UpdateTarget()
    {
        float xTarget;
        float yTarget;
        direction = !direction;

        // If the next target should be to the right
        if(direction)
        {
            if(Player.getPosition().x > transform.position.x)
            {
                xTarget = Player.getPosition().x + Random.Range(minOffset, maxOffset);
            } else
            {
                xTarget = transform.position.x + Random.Range(minOffset, maxOffset);
            }
        } else
        {
            if(Player.getPosition().x < transform.position.x)
            {
                xTarget = Player.getPosition().x - Random.Range(minOffset, maxOffset);
            } else
            {
                xTarget = transform.position.x - Random.Range(minOffset, maxOffset);
            }
        }

        yTarget = Player.getPosition().y + Random.Range(-0.5f, 2f);
        Debug.DrawRay(new Vector3(xTarget, yTarget, 0), Vector2.right, Color.red, Mathf.Infinity, false);
        return new Vector2(xTarget, yTarget);
    }
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
