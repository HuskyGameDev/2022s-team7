using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    Rigidbody2D rb;

    float angle = 25; // Angle to freeze the spear
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float r = Mathf.Abs(transform.rotation.eulerAngles.z % 360); // Gets the rotation value of the spear
        // If the spear hit a wall and is within the angle range to get stuck, freeze the spear.
        if(collision.gameObject.layer == 3 && (r > 360 - angle || r < angle || (r > 180 - angle && r < 180 + angle))) {
            rb.bodyType = RigidbodyType2D.Static;
            gameObject.layer = 3;
        }
    }
}
