using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float bounce = 20f;
    [SerializeField] private string direction = "up";

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.layer == 7)
        {
            switch (direction){
            case "up":
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,1) * bounce, ForceMode2D.Impulse);
                break;
            case "down":
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,-1) * bounce, ForceMode2D.Impulse);
                break;
                case "left":
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1,0) * 2 * bounce, ForceMode2D.Impulse);                
                break;
            case "right":
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1,0) * 2 * bounce, ForceMode2D.Impulse);
                break;
            default:
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,1) * bounce, ForceMode2D.Impulse);
                break;
            }
        }

        if(collision.gameObject.layer == 6)
        {
            Quaternion rotation;
            switch (direction){
            case "up":
                rotation = Quaternion.LookRotation(new Vector2(0,1));
                collision.gameObject.GetComponent<Rigidbody2D>().SetRotation(rotation);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,1) * bounce, ForceMode2D.Impulse);
                break;
            case "down":
                rotation = Quaternion.LookRotation(new Vector2(0,-1));
                collision.gameObject.GetComponent<Rigidbody2D>().SetRotation(rotation);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,-1) * bounce, ForceMode2D.Impulse);
                break;
                case "left":
                rotation = Quaternion.LookRotation(new Vector2(-1,0));
                collision.gameObject.GetComponent<Rigidbody2D>().SetRotation(rotation);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1,0) * bounce, ForceMode2D.Impulse);                
                break;
            case "right":
                rotation = Quaternion.LookRotation(new Vector2(1,0));
                collision.gameObject.GetComponent<Rigidbody2D>().SetRotation(rotation);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1,0) * bounce, ForceMode2D.Impulse);
                break;
            default:
                rotation = Quaternion.LookRotation(new Vector2(0,1));
                collision.gameObject.GetComponent<Rigidbody2D>().SetRotation(rotation);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,1) * bounce, ForceMode2D.Impulse);
                break;
            }
        }
    }


}
