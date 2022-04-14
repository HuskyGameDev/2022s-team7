using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // ========== MOVEMENT ========== //
    private static Rigidbody2D rb;
    private int lookDirection = 1; // 1 is looking to the right, -1 is looking to the left
    [SerializeField] private float playerSpeed = 5.0f;
    public LayerMask GroundLayer;
    private float vX = 0;
    private float vY = 0;

    // ========== DASH ========== //
    [SerializeField] private float dashSpeed = 8.0f;
    [SerializeField] private float dashCooldown = 1f;
    private float dashCooldownTimer = 0;
    private float dashLength = 0.3f; // how long the dash lasts for
    private float dashTimer = 0;
    private bool dashing = false;
    private float dashAccel = 0.1f;

    // ========== JUMP ========== //
    Collider2D collider;
    [SerializeField] float fallMultiplier = 2.5f; // The multiplier to increase the fall speed
    [SerializeField] float lowJumpMultiplier = 2f; // The multiplier to increase the fall speed when the player ends the jump early
    [SerializeField] float jumpVelocity = 5f; // The initial velocity of the jump
    [SerializeField] float gravity = 3f; // The gravity multiplier
     float contactDistance = 0.1f; // Length of the raycast to look for ground
    
    bool jumpButtonHeld = false;
    bool jumpButtonPressed = false;

    // ========== SPEAR ========== //
    public GameObject spear;
    private static bool hasSpear;
    [SerializeField] float spearSpeed;
    private Camera cam;
    [SerializeField] float offset; // The offset amount at which the spear spawns (vertical offset)
    private static float spearDelay = 0.25f; // The time delay after picking up the spear before it can be thrown again
    private static float spearDelayTimer = 0;

    // ========== MANAGEMENT ========== //
    public playerHealth hp; // instance of the hp on the level linked to the player.
    public static Vector2 lastCheckpoint = new Vector2(0,0);



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        cam = Camera.main;
        hasSpear = true;
        gameObject.transform.position = lastCheckpoint;
        rb.gravityScale = gravity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            if(!dashing) {
                dashTimer = dashLength;
                rb.gravityScale = 0;
            }
            Dash();
        }
            
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && hasSpear && spearDelayTimer <= 0) ThrowSpear();

        //Detects if the button to jump is being held, and updates the boolean.
        if (Input.GetButton("Jump"))
        {
            jumpButtonHeld = true;
        }
        else
        {
            jumpButtonHeld = false;
        }
        //Detects if the button to jump has been pressed and the player is not currently moving vertically, and updates the boolean.
        //if (Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0))
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpButtonPressed = true;
        }

        // Timers

        if(dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;
        if (dashTimer > 0) dashTimer -= Time.deltaTime;
        if (spearDelayTimer > 0) spearDelayTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!dashing)
        {
            // Updates vertical velocity if jumping
            Jump();

            // Moves the player horizontally depending on user input
            MovePlayer();
        } else
        {
            Dash();
        }
    }

    // Generates a new Spear instance and throws it toward the mouse
    private void ThrowSpear()
    {
        // Gets the position of the mouse and player
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = rb.position;
        Vector3 playerPositionWorld = cam.WorldToScreenPoint(new Vector3(playerPosition.x, playerPosition.y, 0));

        // Calculates the angle at which the spear should rotate/travel
        float slope = ((mousePosition.y - playerPositionWorld.y) / (mousePosition.x - playerPositionWorld.x));
        Quaternion rotation = Quaternion.LookRotation(new Vector2(mousePosition.x - playerPositionWorld.x, mousePosition.y - playerPositionWorld.y));
        // Fixes the quaternion (Because Unity hates me and puts stuff on the wrong axes because the LookRotation method was designed for 3D space...)
        rotation.z = (mousePosition.x < playerPositionWorld.x) ? rotation.x : -rotation.x;
        rotation.y = 0;
        rotation.x = 0;
        Vector2 slopeV = new Vector2(1, slope);

        // Instantiates the spear and applies rotation + force
        GameObject thrownSpear = Instantiate(spear, playerPosition + new Vector2(0, offset), rotation);
        Rigidbody2D spearRB = thrownSpear.GetComponent<Rigidbody2D>();
        SpriteRenderer spearSprite = thrownSpear.GetComponent<SpriteRenderer>();
        if (mousePosition.x < playerPositionWorld.x) // Adjusts certain variables if the spear is thrown on the left side of the screen
        {
            slopeV *= -1;
            thrownSpear.transform.Rotate(new Vector3(0, 0, 180));
            spearSprite.flipY = false;
        }

        spearRB.SetRotation(rotation);
        spearRB.AddForce(slopeV.normalized * spearSpeed);
        hasSpear = false;
    }


    //checks if the player is going up or down. If the players y axis movement is 0 then the player is on ground
    bool IsGrounded()
    {
        ContactFilter2D filter2D = new ContactFilter2D();
        filter2D.useTriggers = false;

        //Casts a ray to check if there is anything immediatly below the player
        RaycastHit2D[] results = new RaycastHit2D[10];
        collider.Cast(Vector2.down, filter2D, results, contactDistance);

        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider != null)
            {
				if (!hasSpear && hit.transform.gameObject.tag == "Spear") {
					hit.transform.gameObject.SendMessage("playerCollide");
					hasSpear = true;
				}
                return true;
            }
        }
        return false;
    }


    //moves player according to the horizontal key inputs. Found in edit -> project settings -> input manager 
    private void MovePlayer()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * playerSpeed, rb.velocity.y);
        lookDirection = rb.velocity.x < 0 ? -1 : 1;
    }


    //jumps if the player is grounded
    private void Jump()
    {
        if (jumpButtonPressed)
        {
			if (!hasSpear) {
				RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 1.15f, LayerMask.GetMask("Ground"));
				if (hit) {
					if (hit.transform.gameObject.tag == "Spear") {
						hit.transform.gameObject.SendMessage("playerCollide");
					}
				}
			}
            GetComponent<AudioSource>().Play();
            rb.velocity = Vector2.up * jumpVelocity;
            jumpButtonPressed = false;
        }
        //Adjusts the player's downward velocity during a jump
        if (rb.velocity.y < 0) //If the player is falling
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !jumpButtonHeld) //If the player is moving upward, but the jump button is not pressed
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // limits max fall speed
        if(rb.velocity.y <= playerSpeed * -5)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerSpeed * -5);
        }
    }


    private void Dash()
    {
        float targetVelocity = lookDirection == 1 ? dashSpeed : -dashSpeed;

        if(dashTimer > 0)
        {
            dashing = true;
            rb.velocity = new Vector2(targetVelocity, 0);
        } else
        {
            dashing = false;
            dashCooldownTimer = dashCooldown;
            rb.gravityScale = gravity;
        }
    }

    public static void ReturnSpear()
    {
        hasSpear = true;
        spearDelayTimer = spearDelay;
    }

    public static Vector2 getPosition()
    {
        return rb.position;
    }

    //handles all hit detection for the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //basic checking if colliding with an enemy. The layer can be changed for higher amounts of damage once done.
        if(collision.gameObject.layer == 8)
        {
            hp.Damage(1);
        }
        
        //enviormental damage(killboxes for out of bounds or instant death traps)
        if (collision.gameObject.layer == 11)
            {
                hp.Damage(3);
            }
			
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //basic checking if colliding with an enemy.
        if (collision.gameObject.layer == 8)
        {
            hp.Damage(1);
        }
    }

}
