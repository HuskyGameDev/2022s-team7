using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    Rigidbody2D rb;
    bool impact; // Keeps track of whether or not the spear has already impacted something.

    float angle = 40; // Angle to freeze the spear
    bool rotateSpear = false; // Whether or not the spear should be rotating toward the ground
    bool rotated = false; // Whether or not the spear has been successfully rotated
    float prevAngle;
    float curAngle;

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
            // Casts a ray to check if the tip of the spear is colliding with the ground
            if (Physics2D.Raycast(transform.position, transform.right, 1.15f, LayerMask.GetMask("Ground")) && withinRotation)
            {
                // Debug.DrawRay(transform.position, transform.right * 1.15f, Color.yellow, Mathf.Infinity, false); // Can be used to see the ray (turn gizmos on)
                rb.bodyType = RigidbodyType2D.Static;
                gameObject.layer = 3;
            }
            else
            {
                // Debug.DrawRay(transform.position, transform.right * 1.15f, Color.white, Mathf.Infinity, false); // Can be used to see the ray (turn gizmos on)
                rotateSpear = true;
                prevAngle = (transform.rotation.eulerAngles.z + 90) % 360; // Stores the angle of the spear
                impact = true;
            }
        } else if (Physics2D.Raycast(transform.position, transform.right, 1.15f, LayerMask.GetMask("Ground")) && rotated)
        {
            rb.bodyType = RigidbodyType2D.Static;
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
    }
}
