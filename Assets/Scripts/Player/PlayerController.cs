using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2D;

    bool dead = false;

    float maxSpeed = 4f; //The player's max horizontal speed
    float groundAcceleration = 0.2f; //the player's horizontal acceleration on the ground
    float airAcceleration = 0.17f; //the player's horizontal acceleration on the ground
    float quickStopSpeed = 0.5f; //How fast the player stops when holding both left & right on the ground
    float slowStopSpeed = 0.1f; //How fast the player stops holding no keys on the ground
    float verySlowStopSpeed = 0.06f; //how fast the player stops holding no keys in the air
    float jumpSpeed = 6f; //Vertical speed applied when jumping;

    GameObject respawnPoint; //The current spot where the player will respawn

    LayerMask ground;
    RaycastHit2D groundCheck;
    public bool onGround;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D objCollider) //collision with object
    {
        if (objCollider.gameObject.tag == "Respawn")
        {
            respawnPoint = objCollider.gameObject;
        }
    }

    void respawn(float posX, float posY)
    {
        dead = false;
        transform.SetPositionAndRotation(new Vector2(posX, posY), new Quaternion(0, 0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) //if the player is dead, then respawn
            respawn(respawnPoint.transform.position.x, respawnPoint.transform.position.y);

        ground = LayerMask.GetMask("Level");

        //Cast a ray downward
        groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.53f, ground);

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
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
            }

            if ((rb2D.velocity.x == 0) && Input.GetKey("down") || Input.GetKey("s")) //crouch pressed when stopped
            {
                Debug.Log("Crouch here");
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

    public void club(bool strong) //player used a clubs suit card
    {
        if (strong == true)
            Debug.Log("Used a club");
        else
            Debug.Log("Weak club");
    }

    public void diamond(bool strong) //diamonds suit card
    {
        if (strong == true)
            Debug.Log("Used a diamond");
        else
            Debug.Log("Weak diamond");
    }

    public void heart(bool strong) //hearts suit card
    {
        if (strong == true)
            Debug.Log("Used a heart");
        else
            Debug.Log("Weak heart");
    }

    public void spade(bool strong) //spades suit card
    {
        if (strong == true)
            Debug.Log("Used a spade");
        else
            Debug.Log("Weak spade");
    }

    public void joker() //player is out of cards, gets a random effect
    {
        Debug.Log("Joker card");

        //random number between 8 choices
        //1 - 4 being clubs - spades, with 5 -8 being weaker versions of the previous
        int choice = Random.Range(1, 9);

        switch (choice)
        {
            case 1:
                club(true);
                break;
            case 2:
                diamond(true);
                break;
            case 3:
                heart(true);
                break;
            case 4:
                spade(true);
                break;
            case 5:
                club(false);
                break;
            case 6:
                diamond(false);
                break;
            case 7:
                heart(false);
                break;
            case 8:
                spade(false);
                break;
            default:
                Debug.Log("This isn't supposed to happen");
                break;
        }
    }
}