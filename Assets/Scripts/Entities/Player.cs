using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Movement Variables
    Rigidbody2D rb;
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpPower = 5.0f;
    public LayerMask GroundLayer;
    bool isShiftKeyDown;
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
        MovePlayer();
        if(isShiftKeyDown = Input.GetKey(KeyCode.LeftShift))
        {
            dash();
        }
        if (Input.GetButton("Jump")) // Jump is defined in input manager
        {
            Jump();
        }
            
        if (!throwSpear) throwSpear = Input.GetMouseButtonDown(0);
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
    //checks if the player is going up or down. If the players y axis movement is 0 then the player is on ground
    bool IsGrounded()
    {
        float GetVerticalSpeed() => rb.velocity.y; // gets the vertical speed
        if(GetVerticalSpeed() == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //moves player according to the horizontal key inputs. Found in edit -> project settings -> input manager 
    private void MovePlayer()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * playerSpeed, rb.velocity.y);
    }

    //jumps if the player is grounded
    private void Jump()
    {
        if (!IsGrounded())
        {
            return;
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        
        
    }
    // The player dashes in a direction with great speed. Player cannot collide with enemies while dashing
    private void dash()
    {
        rb.velocity = new Vector2(rb.velocity.x * 3, rb.velocity.y);
    }
}
