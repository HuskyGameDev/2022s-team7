using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spear : MonoBehaviour, IPointerClickHandler
{
    Rigidbody2D rb;
    bool impact; // Keeps track of whether or not the spear has already impacted something.

    float angle = 40; // Angle to freeze the spear
    bool rotateSpear = false; // Whether or not the spear should be rotating toward the ground
    bool rotated = false; // Whether or not the spear has been successfully rotated
    bool freeze = true;
    bool canPickUp = false; // Whether or not the player can pick up the spear
    float prevAngle;
    float curAngle;
    bool tipcollsion; // Checks whether the tip is currently colliding with something (for use by other scripts)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        impact = false;
    }

    // Called when the spear collides with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Gets the rotation value of the spear and checks if it's wihin +/- angle degrees of horizontal
        float r = Mathf.Abs(transform.rotation.eulerAngles.z % 360);
        bool withinRotation = r > 360 - angle || r < angle || (r > 180 - angle && r < 180 + angle);

        if (!impact)
        {
            // Casts a ray to check if the tip of the spear is colliding with the ground & within rotation margin. If so, turns spear into a platform
            if (Physics2D.Raycast(transform.position, transform.right, 1.15f, LayerMask.GetMask("Ground")) && withinRotation)
            {
                // Debug.DrawRay(transform.position, transform.right * 1.15f, Color.yellow, Mathf.Infinity, false); // Can be used to see the ray (turn gizmos on)
                canPickUp = true;
                rb.bodyType = RigidbodyType2D.Static;
                gameObject.layer = 3;
            }
            else
            {
                // If the spear has collided with an Enemy (layer 8 == enemy layer)
                if(collision.gameObject.layer == 8)
                {
                    rb.velocity = new Vector2(rb.velocity.x / 5.0f, rb.velocity.y / 5.0f);
                }
                // Debug.DrawRay(transform.position, transform.right * 1.15f, Color.white, Mathf.Infinity, false); // Can be used to see the ray (turn gizmos on)
                rotateSpear = true;
                prevAngle = (transform.rotation.eulerAngles.z + 90) % 360; // Stores the angle of the spear
                impact = true;
                freeze = false;
            }
        } else if (Physics2D.Raycast(transform.position, transform.right, 1.15f, LayerMask.GetMask("Ground")) && rotated)
        {
            // Freezes the spear if the tip hits the ground
            rb.bodyType = RigidbodyType2D.Static;
            canPickUp = true;
        }
        
    }

    private void FixedUpdate()
    {
        if(rotateSpear)
        {
            curAngle = (transform.rotation.eulerAngles.z + 90) % 360; // Gets the current angle of the spear

            // Adds torque to whichever direction the spear is currently rotating
            if(curAngle > prevAngle)
            {
                rb.AddTorque(7);
            } else if (curAngle < prevAngle)
            {
                rb.AddTorque(-7);
            }
        }
    }

    private void Update()
    {
        // If the spear is rotating toward the ground, freeze rotation once the tip of the spear is pointed toward the ground
        float r = Mathf.Abs(transform.rotation.eulerAngles.z + 90) % 360;
        if(rotateSpear && (r < 10 || r > 350))
        {
            rotateSpear = false;
            rb.freezeRotation = true;
            rotated = true;
        }

        // If the y velocity of the spear is 0 and the spear is not rotated properly, freeze it (edge case control) (sorry for the mess of an if statement)
        if(rb.velocity.y == 0 && !rotated && !freeze && (rb.velocity.x == 0 || rotateSpear) && !Physics2D.Raycast(transform.position, transform.right, 1.15f, LayerMask.GetMask("Ground")))
        {
            rb.bodyType = RigidbodyType2D.Static;
            canPickUp = true;
            freeze = true;
        }

        // If the spear is rotated + frozen but not static, fix it (debugging / edge case)
        if(!canPickUp && rotated && rb.velocity == Vector2.zero)
        {
            canPickUp = true;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    // Gets called whenever the spear is clicked on
    public void OnPointerClick(PointerEventData eventData)
    {
        if(canPickUp)
        {
            Player.ReturnSpear();
            Destroy(gameObject);
        }
    }

    public bool getTipCollision(GameObject obj)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1.15f, LayerMask.GetMask("Ground", "Enemy"));
        if(hit.transform == null)
        {
            return false;
        }
        if(hit.transform.gameObject.Equals(obj) && rb.velocity.magnitude > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
