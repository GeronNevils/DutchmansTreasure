using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    AudioSource aldoNova;
    public AudioClip fallDeathSound;
    public AudioClip hitDeathSound;
    public AudioClip respawnSound;
    public AudioClip discardSound;
    public AudioClip clubSound;
    public AudioClip diamondSound;
    public AudioClip heartSound;
    public AudioClip spadeSound;

    Rigidbody2D rb2D;
    cardController cardCon;
    SpriteRenderer sr;
    Animator anim;
    StatTracker stats;

    GameUI controlFreeze;

    public bool dead = false;

    float maxSpeed = 4f; //The player's max horizontal speed
    float groundAcceleration = 0.2f; //the player's horizontal acceleration on the ground
    float airAcceleration = 0.17f; //the player's horizontal acceleration on the ground
    float quickStopSpeed = 0.5f; //How fast the player stops when holding both left & right on the ground
    float slowStopSpeed = 0.1f; //How fast the player stops holding no keys on the ground
    float verySlowStopSpeed = 0.06f; //how fast the player stops holding no keys in the air
    float jumpSpeed = 5.7f; //Vertical speed applied when jumping;

    int respawnDelay = 0; //The delay between death and respawn

    GameObject respawnPoint; //The current spot where the player will respawn
    bool respawnSetBefore = false;

    GameObject spawnCard; //GameObjects to keep track of when instantiating
    GameObject spawnHitbox;
    GameObject spawnHitboxExtra;

    public GameObject gotHitParticles;
    public GameObject respawnParticles;
    public GameObject useCardParticles;
    public GameObject discardParticles;
    public GameObject clubParticles;

    public bool cardActive; //if a card effect is currently in use

    public GameObject clubCard; //the club card and its hitboxes
    public GameObject clubFallHitbox;
    public GameObject clubLandHitbox;
    bool ridingClub = false; //if the player is currently riding on a card
    int groundTimer = 0; //wait time when landing
    float clubFallSpeed = -10f; //how fast the player falls riding on the club

    public GameObject diamondCard; //the diamond card

    public GameObject heartCard; //the heart card
    public bool shieldsUp = false; //if the player's shield is active
    int shieldDuration = 180; //Duration shield will be active
    int shieldTimeLeft = 0; //Remaining duration of shield

    public GameObject spadeCard; //the spade card
    float spadeJumpSpeed = 8f; //Vertical speed from using a spade card (trampoline)

    LayerMask ground;
    RaycastHit2D groundCheck;
    RaycastHit2D groundCheck2;
    public bool onGround;

    Collider2D fugginOffsets;
    float xOff;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        cardCon = GetComponent<cardController>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        controlFreeze = GameObject.FindGameObjectWithTag("UIcontrol").GetComponent<GameUI>();
        stats = GameObject.FindGameObjectWithTag("StatTracker").GetComponent<StatTracker>();

        aldoNova = GetComponent<AudioSource>();

        fugginOffsets = GetComponent<Collider2D>();
        xOff = fugginOffsets.offset.x;

        stats.cleanOut();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D objCollider)
    {
        if (!dead)
        {
            if (objCollider.gameObject.tag == "Respawn") //set current respawn point
            {
                respawnPoint = objCollider.gameObject;
                stats.addRoomPos(objCollider.gameObject.transform.parent.transform.position.x,
                                 objCollider.gameObject.transform.parent.transform.position.y);

                Destroy(objCollider);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D objCollider) //collision with object
    {
        if (!dead)
        {           
            if (((objCollider.gameObject.tag == "Enemy" ||
                      objCollider.gameObject.tag == "Cannonball")
                && shieldsUp == false && ridingClub == false) ||
                      (objCollider.gameObject.tag == "Boss")) //hit enemy with no shield
            {
                setDead(); //die

                aldoNova.clip = hitDeathSound;
                aldoNova.PlayOneShot(hitDeathSound, 1f);

                //particles
                Instantiate(gotHitParticles, transform.position, new Quaternion(0, 0, 0, 0));
            }
            else if (objCollider.gameObject.tag == "KillZone") //hit out-of-bounds
            {
                setDead(); //die
            }
        }
    }

    public void caughtByMimic() //caught by a mimic chest
    {
        setDead();
        sr.enabled = false;
    }

    void setDead()
    {
        dead = true;
        anim.SetBool("isDead", true);
        effectCancel();
        respawnDelay = 300;
        stats.addDeath(transform.position.x, transform.position.y);      
    }

    void respawn(float posX, float posY) //respawn player at respawn point
    {
        dead = false;
        effectCancel();
        anim.SetBool("isDead", false);
        if (sr.enabled == true)
        {
            //spawn particles
            Instantiate(respawnParticles, transform.position, new Quaternion(0, 0, 0, 0));
        }

        aldoNova.clip = respawnSound;
        aldoNova.PlayOneShot(respawnSound, 1f);

        transform.SetPositionAndRotation(new Vector2(posX, posY), new Quaternion(0, 0, 0, 0));
        sr.enabled = true;
        //spawn particles
        Instantiate(respawnParticles, transform.position, new Quaternion(0, 0, 0, 0));
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
        if (sr.flipX == true)
        {
            fugginOffsets.offset = new Vector2((xOff * -1), fugginOffsets.offset.y);
            groundCheck2 = Physics2D.Raycast(new Vector2(
                                                     (transform.position.x - 0.25f),
                                                     transform.position.y),
                                                     Vector2.down, 0.89f, ground);
        }
        else
        {
            fugginOffsets.offset = new Vector2(xOff, fugginOffsets.offset.y);
            groundCheck2 = Physics2D.Raycast(new Vector2(
                                                     (transform.position.x + 0.25f),
                                                     transform.position.y),
                                                     Vector2.down, 0.89f, ground);
        }

        if (dead && respawnDelay == 0) //if the player is dead and the delay is done, then respawn
            respawn(respawnPoint.transform.position.x, respawnPoint.transform.position.y);
        //Implement cooldown and death animation

        if (respawnDelay > 0) //if delay is active, decrease time remaining
            respawnDelay--;

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

        if (groundTimer > 0)
            groundTimer--;

        ground = LayerMask.GetMask("Level");

        //Cast a ray downward
        groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.89f, ground);
       
        
        if (groundCheck.collider != null || groundCheck2.collider != null) //the player is on the ground
        {
            onGround = true;
            anim.SetBool("isGrounded", true);
        }
        else //the player is in the air
        {
            onGround = false;
            anim.SetBool("isGrounded", false);
        }

        if (onGround && rb2D.velocity.y < -9f && !ridingClub) //hit ground too fast
        {
            aldoNova.clip = fallDeathSound;
            aldoNova.PlayOneShot(aldoNova.clip, 1f);

            if (!dead)
                setDead(); //die
            Instantiate(gotHitParticles, transform.position, new Quaternion(0, 0, 0, 0));
        }

        if (Input.GetKeyDown("r") && !controlFreeze.freeze) //input respawn
        {
            respawn(respawnPoint.transform.position.x, respawnPoint.transform.position.y);
        }

        if ((Input.GetKeyDown("l") || 
             Input.GetKeyDown(KeyCode.Keypad3) ||
             Input.GetMouseButtonDown(1)) && cardActive == true) //cancel current card effects
        {
            Instantiate(discardParticles, transform.position, new Quaternion(0, 0, 0, 0));
            effectCancel();
            aldoNova.clip = discardSound;
            aldoNova.PlayOneShot(discardSound, 1f);
        }
        else if ((Input.GetKeyDown("l") || 
                  Input.GetKeyDown(KeyCode.Keypad3) ||
                  Input.GetMouseButtonDown(1)) && cardActive == false && !controlFreeze.freeze) //discard card
        {
            Instantiate(discardParticles, transform.position, new Quaternion(0, 0, 0, 0));
            cardCon.discard();
            aldoNova.clip = discardSound;
            aldoNova.PlayOneShot(discardSound, 1f);
        }

        if (dead == true) //player is dead
        {
            if (!onGround) //in air
            {
                transform.Rotate(0, 0, 500 * Time.deltaTime); //spin
            }
            else //on ground
            {
                if (rb2D.velocity.y >= -0.5f)
                {
                    transform.SetPositionAndRotation(transform.position, new Quaternion(0, 0, 0, 0));
                }
            }

            //stop very very slowly
            float temp = rb2D.velocity.x;
            if (temp < 0)
            {
                temp += 0.03f;
                if (temp > 0)
                    temp = 0;
            }
            else
            {
                temp -= 0.03f;
                if (temp < 0)
                    temp = 0;
            }

            rb2D.velocity = new Vector2(temp, rb2D.velocity.y);

            if (onGround && rb2D.velocity.y < -0.5f) //fell on the ground fast
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, (rb2D.velocity.y / 2) * -1); //bounce
            }
        }
        else if (onGround && !ridingClub && !dead && !controlFreeze.freeze) //player is on the ground, and not dead
        {
            if (Input.GetKeyDown("up") || Input.GetKeyDown("w")) //jump pressed
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
                anim.SetBool("isGrounded", false);
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

            //set direction to face
            if (rb2D.velocity.x < 0) //traveling left
                sr.flipX = true; //face left
            else if (rb2D.velocity.x > 0) //traveling right
                sr.flipX = false; //face right
        }
        else if (!onGround && !ridingClub && !dead && !controlFreeze.freeze) //player is not on the ground, and is not on a club card
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
                sr.flipX = true; //face left

                float temp = rb2D.velocity.x;
                temp -= airAcceleration;

                if (temp < (maxSpeed * -1))
                    temp = (maxSpeed * -1);

                rb2D.velocity = new Vector2(temp, rb2D.velocity.y);
            }
            else if (Input.GetKey("right") || Input.GetKey("d")) //move right
            {
                sr.flipX = false; //face right

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
        else if (ridingClub == true && !dead) //the player is riding the club card
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

                    aldoNova.clip = clubSound;
                    aldoNova.PlayOneShot(clubSound, 1f);

                    Instantiate(clubParticles, new Vector3(
                                                           transform.position.x,
                                                           (transform.position.y - 0.5f),
                                                           transform.position.z), new Quaternion(0, 0, 0, 0));
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

        //set animation idle/not idle
        if (rb2D.velocity.x == 0.0f)
            anim.SetBool("isIdle", true);
        else
            anim.SetBool("isIdle", false);
    }

    public void club(bool strong) //player used a clubs suit card
    {
        stats.addUsedCard(transform.position.x, transform.position.y, "Clubs");

        Instantiate(useCardParticles, transform.position, new Quaternion(0, 0, 0, 0));
        rb2D.velocity = new Vector2(0f, 0f);
        ridingClub = true;
        cardActive = true;

        spawnCard = Instantiate(clubCard, rb2D.transform.position, new Quaternion(0, 0, 0, 0));
        spawnHitbox = Instantiate(clubFallHitbox, rb2D.transform.position, new Quaternion(0, 0, 0, 0));
    }

    public void diamond(bool strong) //diamonds suit card
    {
        stats.addUsedCard(transform.position.x, transform.position.y, "Diamonds");

        Instantiate(useCardParticles, transform.position, new Quaternion(0, 0, 0, 0));

        aldoNova.clip = diamondSound;
        aldoNova.PlayOneShot(diamondSound, 1f);

        if (strong == true)
        {
            GameObject c = Instantiate(diamondCard, rb2D.transform.position, new Quaternion(0, 0, 0, 0));

            if (sr.flipX == false) //facing right
            {
                c.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 0f);
                rb2D.velocity = new Vector2(rb2D.velocity.x - 5f, rb2D.velocity.y);
            }
            else //facing left
            {
                c.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 0f);
                rb2D.velocity = new Vector2(rb2D.velocity.x + 5f, rb2D.velocity.y);
            }
        }
        else
        {
            GameObject c = Instantiate(diamondCard, rb2D.transform.position, new Quaternion(0, 0, 0, 0));
            c.GetComponent<Rigidbody2D>().gravityScale = 1;

            if (sr.flipX == false) //facing right
            {
                c.GetComponent<Rigidbody2D>().velocity = new Vector2(3f, 7f);
                rb2D.velocity = new Vector2(rb2D.velocity.x - 5f, rb2D.velocity.y);
            }
            else //facing left
            {
                c.GetComponent<Rigidbody2D>().velocity = new Vector2(-3f, 7f);
                rb2D.velocity = new Vector2(rb2D.velocity.x + 5f, rb2D.velocity.y);
            }
        }
    }

    public void heart(bool strong) //hearts suit card
    {
        stats.addUsedCard(transform.position.x, transform.position.y, "Hearts");

        Instantiate(useCardParticles, transform.position, new Quaternion(0, 0, 0, 0));
        aldoNova.clip = heartSound;
        aldoNova.PlayOneShot(heartSound, 1f);

        if (strong == true)
        {
            shieldTimeLeft = shieldDuration;
        }
        else
        {
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
        stats.addUsedCard(transform.position.x, transform.position.y, "Spades");

        Instantiate(useCardParticles, transform.position, new Quaternion(0, 0, 0, 0));

        aldoNova.clip = spadeSound;
        aldoNova.PlayOneShot(spadeSound, 1f);

        rb2D.velocity = new Vector2(0f, 0f); //stop moving
        GameObject sp = Instantiate(spadeCard, new Vector3(rb2D.transform.position.x,
                                                       rb2D.transform.position.y - 0.5f,
                                                       rb2D.transform.position.z), new Quaternion(0, 0, 0, 0));

        if (strong == true)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, spadeJumpSpeed);
        }
        else
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, (spadeJumpSpeed * 0.5f));
        }
    }

    public void joker() //player is out of cards, gets a random effect
    {
        //random number between 7 choices
        //1 - 4 being clubs - spades, with 5 - 7 being weaker versions of the previous
        int choice = Random.Range(1, 8);

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
                diamond(false);
                break;
            case 6:
                heart(false);
                break;
            case 7:
                spade(false);
                break;
            default:
                Debug.Log("This isn't supposed to happen");
                break;
        }
    }
}