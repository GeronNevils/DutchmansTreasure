using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2D;

    float maxSpeed = 4f; //The player's max horizontal speed
    float groundAcceleration = 0.2f; //the player's horizontal acceleration on the ground
    float airAcceleration = 0.17f; //the player's horizontal acceleration on the ground
    float quickStopSpeed = 0.5f; //How fast the player stops when holding both left & right on the ground
    float slowStopSpeed = 0.1f; //How fast the player stops holding no keys on the ground
    float verySlowStopSpeed = 0.06f; //how fast the player stops holding no keys in the air

    public bool onGround;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LayerMask ground = LayerMask.GetMask("Level");

        //Cast a ray downward
        RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.7f, ground);

        if (groundCheck.collider != null) //the player is on the ground
        {
            onGround = true;
        }
        else //the player is in the air
        {
            onGround = false;
        }

        if (onGround) //player is on the ground
        {
            if (Input.GetKeyDown("up") || Input.GetKeyDown("w")) //jump pressed
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, 5f);
            }

            if ((Input.GetKey("left") || Input.GetKey("a")) && //both left and right are pressed
                (Input.GetKey("right") || Input.GetKey("d")))
            {
                //stop quickly
                float temp = rb2D.velocity.x;
                if (temp < 0)
                {
                    temp += quickStopSpeed;
                    if (temp > 0)
                        temp = 0;
                }
                else
                {
                    temp -= quickStopSpeed;
                    if (temp < 0)
                        temp = 0;
                }

                rb2D.velocity = new Vector2(temp, rb2D.velocity.y);
            }
            else if (Input.GetKey("left") || Input.GetKey("a")) //move left
            {
                float temp = rb2D.velocity.x;
                temp -= groundAcceleration;

                if (temp < (maxSpeed * -1))
                    temp = (maxSpeed * -1);

                rb2D.velocity = new Vector2(temp, rb2D.velocity.y);
            }
            else if (Input.GetKey("right") || Input.GetKey("d")) //move right
            {
                float temp = rb2D.velocity.x;
                temp += groundAcceleration;

                if (temp > maxSpeed)
                    temp = maxSpeed;

                rb2D.velocity = new Vector2(temp, rb2D.velocity.y);
            }
            else if (!Input.anyKey) //no keys are pressed
            {
                //stop slowly
                float temp = rb2D.velocity.x;
                if (temp < 0)
                {
                    temp += slowStopSpeed;
                    if (temp > 0)
                        temp = 0;
                }
                else
                {
                    temp -= slowStopSpeed;
                    if (temp < 0)
                        temp = 0;
                }

                rb2D.velocity = new Vector2(temp, rb2D.velocity.y);
            }
        }
        else if (!onGround) //player is not on the ground
        {
            if ((Input.GetKey("left") || Input.GetKey("a")) && //both left and right are pressed
                (Input.GetKey("right") || Input.GetKey("d")))
            {
                //stop slowly
                float temp = rb2D.velocity.x;
                if (temp < 0)
                {
                    temp += slowStopSpeed;
                    if (temp > 0)
                        temp = 0;
                }
                else
                {
                    temp -= slowStopSpeed;
                    if (temp < 0)
                        temp = 0;
                }

                rb2D.velocity = new Vector2(temp, rb2D.velocity.y);
            }
            else if (Input.GetKey("left") || Input.GetKey("a")) //move left
            {
                float temp = rb2D.velocity.x;
                temp -= airAcceleration;

                if (temp < (maxSpeed * -1))
                    temp = (maxSpeed * -1);

                rb2D.velocity = new Vector2(temp, rb2D.velocity.y);
            }
            else if (Input.GetKey("right") || Input.GetKey("d")) //move right
            {
                float temp = rb2D.velocity.x;
                temp += airAcceleration;

                if (temp > maxSpeed)
                    temp = maxSpeed;

                rb2D.velocity = new Vector2(temp, rb2D.velocity.y);
            }
            else if (!Input.anyKey) //no keys are pressed
            {
                //stop very slowly
                float temp = rb2D.velocity.x;
                if (temp < 0)
                {
                    temp += verySlowStopSpeed;
                    if (temp > 0)
                        temp = 0;
                }
                else
                {
                    temp -= verySlowStopSpeed;
                    if (temp < 0)
                        temp = 0;
                }

                rb2D.velocity = new Vector2(temp, rb2D.velocity.y);
            }
        }
    }
}
