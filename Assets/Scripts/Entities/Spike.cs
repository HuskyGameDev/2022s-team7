using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static playerHealth;

public class Spike : MonoBehaviour
{
	Rigidbody2D rb;
	bool colli;
    public playerHealth hp; // instance of the hp on the level linked to the player.
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	
	private void OnCollisionEnter2D(Collision2D collision)
    {
        // Gets the rotation value of the spear and checks if it's wihin +/- angle degrees of horizontal
        if (collision.gameObject.tag == "Player") {
            hp.Damage(3);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
