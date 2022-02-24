using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static playerHealth;

public class Spike : MonoBehaviour
{
	Rigidbody2D rb;
	bool colli;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
	
	private void OnCollisionEnter2D(Collision2D collision)
    {
        // Gets the rotation value of the spear and checks if it's wihin +/- angle degrees of horizontal
        if (collision.gameObject.tag == "Player") {
			// Player.playerh.Damage(1);
			// To be implemented once player instantiation is finished
		}
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
