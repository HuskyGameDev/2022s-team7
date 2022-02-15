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

    //dash variables
    private float dashSpeed = 3;
    private float dashTime = 0.5f; // how long the dash lasts for
    private Coroutine dodging;

    //Spear Variables
    public GameObject spear;
    bool hasSpear;
    bool throwSpear;
    [SerializeField] float spearSpeed;
    private Camera cam;
    [SerializeField] float offset;


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
        if (Input.GetKeyDown(KeyCode.LeftShift) && dodging == null)
        {
            dodging = StartCoroutine(Dash());
        }
        
        if (Input.GetButton("Jump")) // Jump is defined in input manager
        {
            Jump();
        }
            
        if (Input.GetMouseButtonDown(0)) ThrowSpear();
    }


    // Generates a new Spear instance and throws it toward the mouse
    private void ThrowSpear()
    {
        // Gets the position of the mouse and player
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = rb.position;
        Vector3 playerPositionWorld = cam.WorldToScreenPoint(new Vector3(playerPosition.x, playerPosition.y, 0));

        // Calculates the angle at which the spear should rotate/travel
        float slope = ((mousePosition.y - playerPositionWorld.y) / (mousePosition.x - playerPositionWorld.x));
        Quaternion rotation = Quaternion.LookRotation(new Vector2(mousePosition.x - playerPositionWorld.x, mousePosition.y - playerPositionWorld.y));
        // Fixes the quaternion (Because Unity hates me and puts stuff on the wrong axes because the LookRotation method was designed for 3D space...)
        rotation.z = (mousePosition.x < playerPositionWorld.x) ? rotation.x : -rotation.x;
        rotation.y = 0;
        rotation.x = 0;
        Vector2 slopeV = new Vector2(1, slope);

        // Instantiates the spear and applies rotation + force
        GameObject thrownSpear = Instantiate(spear, playerPosition + new Vector2(0, offset), rotation);
        Rigidbody2D spearRB = thrownSpear.GetComponent<Rigidbody2D>();
        SpriteRenderer spearSprite = thrownSpear.GetComponent<SpriteRenderer>();
        if (mousePosition.x < playerPositionWorld.x) // Adjusts certain variables if the spear is thrown on the left side of the screen
        {
            slopeV *= -1;
            thrownSpear.transform.Rotate(new Vector3(0, 0, 180));
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
    private IEnumerator Dash()
    {
        var endOfFrame = new WaitForEndOfFrame();

        for (float timer = 0; timer < dashTime; timer += Time.deltaTime)
        {
            rb.velocity = new Vector2(rb.velocity.x * dashSpeed, rb.velocity.y);
            yield return endOfFrame;
        }
        dodging = null;
    }
}
