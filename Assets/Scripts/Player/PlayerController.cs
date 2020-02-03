using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2D;
    cardController cardCon;

    bool dead = false;

    float maxSpeed = 4f; //The player's max horizontal speed
    float groundAcceleration = 0.2f; //the player's horizontal acceleration on the ground
    float airAcceleration = 0.17f; //the player's horizontal acceleration on the ground
    float quickStopSpeed = 0.5f; //How fast the player stops when holding both left & right on the ground
    float slowStopSpeed = 0.1f; //How fast the player stops holding no keys on the ground
    float verySlowStopSpeed = 0.06f; //how fast the player stops holding no keys in the air
    float jumpSpeed = 5.5f; //Vertical speed applied when jumping;

    GameObject respawnPoint; //The current spot where the player will respawn

    GameObject spawnCard; //GameObjects to keep track of when instantiating
    GameObject spawnCardExtra;
    GameObject spawnHitbox;
    GameObject spawnHitboxExtra;

    public bool cardActive; //if a card effect is currently in use

    public GameObject clubCard; //the club card and its hitboxes
    public GameObject clubFallHitbox;
    public GameObject clubLandHitbox;
    bool ridingClub = false; //if the player is currently riding on a card
    int groundTimer = 0; //wait time when landing
    float clubFallSpeed = -10f; //how fast the player falls riding on the club

    public GameObject diamondCard; //the diamond card

    public GameObject heartCard; //the heart card
    bool shieldsUp = false; //if the player's shield is active
    int shieldDuration = 300; //Duration shield will be active
    int shieldTimeLeft = 0; //Remaining duration of shield

    public GameObject spadeCard; //the spade card
    float spadeJumpSpeed = 8f; //Vertical speed from using a spade card (trampoline)
    int cardVisualDuration = 0; //How long the trampoline will be visual

    LayerMask ground;
    RaycastHit2D groundCheck;
    public bool onGround;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cardCon = GetComponent<cardController>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D objCollider) //collision with object
    {
        if (objCollider.gameObject.tag == "Respawn") //set current respawn point
        {
            respawnPoint = objCollider.gameObject;
        }
        else if (objCollider.gameObject.tag == "Enemy" && shieldsUp == false && ridingClub == false) //hit enemy with no shield
        {
            dead = true;
        }
        else if (objCollider.gameObject.tag == "KillZone")
        {
            effectCancel();
            dead = true;
        }
    }

    void respawn(float posX, float posY) //respawn player at respawn point
    {
        dead = false;
        transform.SetPositionAndRotation(new Vector2(posX, posY), new Quaternion(0, 0, 0, 0));
        rb2D.velocity = new Vector2(0f, 0f);
    }

    void effectCancel() //cancels current card effect
    {
        if (shieldsUp == true) //cancel shield
        {
            shieldTimeLeft = 0;
            shieldsUp = false;
            Destroy(spawnCard);
            cardActive = false;
        }
        else if (ridingClub == true) //cancel club
        {
            ridingClub = false;
            Destroy(spawnCard);
            Destroy(spawnHitbox);
            Destroy(spawnHitboxExtra);
            rb2D.velocity = new Vector2(0f, 0f);
            cardActive = false;
            groundTimer = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) //if the player is dead, then respawn
            respawn(respawnPoint.transform.position.x, respawnPoint.transform.position.y);
        //Implement cooldown and death animation

        if (shieldTimeLeft > 0) //if shield is active, decrease time remaining
        {
            if (shieldTimeLeft == 1) //turn off active effects
            {
                cardActive = false;
                shieldsUp = false;
                Destroy(spawnCard);
            }

            shieldTimeLeft--;
        }

        if (cardVisualDuration > 0) //for keeping the spade card on screen
            cardVisualDuration--;
        else if (cardVisualDuration == 0)
            Destroy(spawnCardExtra);

        if (groundTimer > 0)
            groundTimer--;

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

        if ((Input.GetKeyDown("l") || Input.GetKeyDown("1")) && cardActive == true) //cancel current card effects
        {
            effectCancel();
        }
        else if ((Input.GetKeyDown("l") || Input.GetKeyDown("1")) && cardActive == false) //discard card
        {
            cardCon.discard();
        }

        if (onGround && !ridingClub) //player is on the ground
        {
            if (Input.GetKeyDown("up") || Input.GetKeyDown("w")) //jump pressed
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
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
        else if (!onGround && !ridingClub) //player is not on the ground, and is not on a club card
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
        else if (ridingClub == true) //the player is riding the club card
        {
            if (!onGround) //in the air
            {
                rb2D.velocity = new Vector2(0f, clubFallSpeed);
            }
            else //hit the ground
            {
                rb2D.velocity = new Vector2(0f, 0f);
                if (groundTimer == 0) //prevent infinite spawning
                {
                    spawnHitboxExtra = Instantiate(clubLandHitbox, rb2D.transform.position, new Quaternion(0, 0, 0, 0));
                    groundTimer = 30;
                }
                Destroy(spawnHitbox);

                if (groundTimer == 1)
                {
                    Destroy(spawnCard);
                    Destroy(spawnHitboxExtra);
                    ridingClub = false;
                    cardActive = false;
                }
            }
        }
    }

    public void club(bool strong) //player used a clubs suit card
    {
        Debug.Log("Used a club");
        rb2D.velocity = new Vector2(0f, 0f);
        ridingClub = true;
        cardActive = true;

        spawnCard = Instantiate(clubCard, rb2D.transform.position, new Quaternion(0, 0, 0, 0));
        spawnHitbox = Instantiate(clubFallHitbox, rb2D.transform.position, new Quaternion(0, 0, 0, 0));
    }

    public void diamond(bool strong) //diamonds suit card
    {
        //This needs adjusting for when player direction is kept track of

        if (strong == true)
        {
            Debug.Log("Used a diamond");
            GameObject c = Instantiate(diamondCard, rb2D.transform.position, new Quaternion(0, 0, 0, 0));
            c.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f); //change to depend on player direction

            rb2D.velocity = new Vector2(rb2D.velocity.x - 5f, rb2D.velocity.y); //change to push player backwards
        }
        else
        {
            Debug.Log("Weak diamond");
            GameObject c = Instantiate(diamondCard, rb2D.transform.position, new Quaternion(0, 0, 0, 0));
            c.GetComponent<Rigidbody2D>().gravityScale = 1;
            c.GetComponent<Rigidbody2D>().velocity = new Vector2(3f, 7f); //change to depend on player direction

            rb2D.velocity = new Vector2(rb2D.velocity.x - 5f, rb2D.velocity.y); //change as well
        }
    }

    public void heart(bool strong) //hearts suit card
    {
        if (strong == true)
        {
            Debug.Log("Used a heart");
            shieldTimeLeft = shieldDuration;
        }
        else
        {
            Debug.Log("Weak heart");
            shieldTimeLeft = (shieldDuration / 2);
        }

        if (onGround == false && rb2D.velocity.y < 0f) //player is not on the ground
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0f); //stop falling to protect from fall damage

        spawnCard = Instantiate(heartCard, rb2D.position, new Quaternion(0, 0, 0, 0));
        shieldsUp = true;
        cardActive = true;
    }

    public void spade(bool strong) //spades suit card
    {
        rb2D.velocity = new Vector2(0f, 0f); //stop moving
        spawnCardExtra = Instantiate(spadeCard, new Vector3(rb2D.transform.position.x,
                                                       rb2D.transform.position.y - 0.5f,
                                                       rb2D.transform.position.z), new Quaternion(0, 0, 0, 0));

        cardVisualDuration = 25;

        if (strong == true)
        {
            Debug.Log("Used a spade");
            rb2D.velocity = new Vector2(rb2D.velocity.x, spadeJumpSpeed);
        }
        else
        {
            Debug.Log("Weak spade");
            rb2D.velocity = new Vector2(rb2D.velocity.x, (spadeJumpSpeed * 0.5f));
        }
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