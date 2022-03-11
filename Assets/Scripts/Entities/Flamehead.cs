using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamehead : MonoBehaviour
{

    Vector2 target;
    bool getTarget;
    [SerializeField] float error;
    Rigidbody2D rb;
    [SerializeField] float maxVelocity; // The maximum velocity at which the flamehead can move
    float minVelocity = 1;
    [SerializeField] float adjust; // The amount by which to multiply/divide the velocity (MUST be greater than 1 and smaller than maxVelocity)
    [SerializeField] bool direction; // The direction of travel. False is left, True is right.
    [SerializeField] float minOffset;
    [SerializeField] float maxOffset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateTarget();
    }

    // Update is called once per frame
    void Update()
    {
        getTarget = true;
    }

    private void FixedUpdate()
    {
        // If the flamehead is vaguely near the target, retarget
        if (Mathf.Abs(transform.position.x - target.x) < error && Mathf.Abs(transform.position.y - target.y) < error)
        {
            target = UpdateTarget();
            getTarget = false;
        } else
        {
            getTarget = true;
        }

        UpdateVelocity();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 6 corresponds to the "Weapon" layer, so the if statement is checking if the Flamehead has collided with a weapon
        if(collision.gameObject.layer == 6 && collision.gameObject.GetComponent<Spear>().getTipCollision(gameObject))
        {
            Destroy(gameObject);
        }
        if(Physics2D.Raycast(transform.position, Vector2.right, 1.15f, LayerMask.GetMask("Ground")))
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            direction = true;
        } else if (Physics2D.Raycast(transform.position, Vector2.left, 1.15f, LayerMask.GetMask("Ground")))
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            direction = false;
        } 
        else
        {
           rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }
        target = UpdateTarget();
    }

    /** 
    * Updates the velocity of the Flamehead
    * This method exists only for code organization
    * It's such a mess of if statements, but I couldn't figure out any other way to do this
    * BUT HEY IT WORKS
    * I sincerely apologize to anyone who has to try and update this
    **/
    private void UpdateVelocity()
    {
        /****** X VELOCITY ******/
        // If the target is to the left
        if (target.x < rb.position.x && (rb.velocity.magnitude < maxVelocity || rb.velocity.x > 0))
        {
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

        }
        // If the target is to the right
        else if (target.x > rb.position.x && (rb.velocity.magnitude < maxVelocity || rb.velocity.x < 0))
        {
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
        }


        /****** Y VELOCITY ******/
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
    }

    private Vector2 UpdateTarget()
    {
        Debug.Log("retargeting");
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
}
