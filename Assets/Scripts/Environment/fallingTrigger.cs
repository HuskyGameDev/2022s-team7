using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingTrigger : MonoBehaviour
{
    Rigidbody2D rb;
    bool active = false;
    void Start()
    {
        active = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            rb.isKinematic = false;
            active = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if hit ground
        if (collision.gameObject.layer == 3 && active == true)
        {
            Destroy(gameObject);
        }
        //if spear thrown
        if (collision.gameObject.layer == 6)
        {
            rb.isKinematic = false;
            active = true;
        }
    }
}
