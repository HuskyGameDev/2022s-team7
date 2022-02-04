using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    Rigidbody2D rb;
    bool impact; // Keeps track of whether or not the spear has already impacted something.

    float angle = 40; // Angle to freeze the spear
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
            // Casts a ray to check if the tip of the spear is colliding with an object
            if (Physics2D.Raycast(transform.position, transform.right, 1.15f, LayerMask.GetMask("Ground")) && withinRotation)
            {
                // Debug.DrawRay(transform.position, transform.right * 1.15f, Color.yellow, Mathf.Infinity, false); // Can be used to see the ray (turn gizmos on)
                rb.bodyType = RigidbodyType2D.Static;
                gameObject.layer = 3;
            }
            else
            {
                // Debug.DrawRay(transform.position, transform.right * 1.15f, Color.white, Mathf.Infinity, false); // Can be used to see the ray (turn gizmos on)
                impact = true;
            }
        }
    }
}
