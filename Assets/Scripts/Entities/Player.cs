using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Movement Variables
    Rigidbody2D rb;

    //Spear Variables
    public GameObject spear;
    bool hasSpear;
    bool throwSpear;
    public float spearSpeed;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        hasSpear = true;
        throwSpear = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!throwSpear) throwSpear = Input.GetMouseButtonDown(0);
    }

    // FixedUpdate is called at a fixed time interval
    private void FixedUpdate()
    {
        if(throwSpear)
        {
            throwSpear = false;
            ThrowSpear();
        }
    }

    private void ThrowSpear()
    {
        // Gets the position of the mouse and player
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = rb.position;
        Vector3 playerPositionWorld = cam.WorldToScreenPoint(new Vector3(playerPosition.x, playerPosition.y, 0));

        // Calculates the angle at which the spear should rotate/travel
        float slope = ((mousePosition.y - playerPositionWorld.y) / (mousePosition.x - playerPositionWorld.x));
        float rotation = Mathf.Tan((mousePosition.y - playerPositionWorld.y) / (mousePosition.x - playerPositionWorld.x)) * Mathf.Rad2Deg;
        Vector2 slopeV = new Vector2(1, slope);

        Debug.Log("rotation: " + rotation);

        // Instantiates the spear and applies rotation + force
        GameObject thrownSpear = Instantiate(spear, playerPosition, Quaternion.identity);
        Rigidbody2D spearRB = thrownSpear.GetComponent<Rigidbody2D>();
        SpriteRenderer spearSprite = thrownSpear.GetComponent<SpriteRenderer>();
        if (mousePosition.x < playerPositionWorld.x) // Adjusts certain variables if the spear is thrown on the left side of the screen
        {
            slopeV *= -1;
            rotation += 180;
            spearSprite.flipY = false;
        }
        spearRB.SetRotation(rotation);
        spearRB.AddForce(slopeV.normalized * spearSpeed);

    }
}
