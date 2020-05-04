using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    AudioSource asas;
    public AudioClip fireSound;

    public GameObject fireParticles;

    public GameObject projectile; //the cannonball
    public GameObject spawnSpot; //where the cannonball spawns

    public float cannonBallSpeed = 4f;

    public int fireCooldown = 300; //for stationary and targeting cannons
    int setCooldown; //holds the original cooldown value

    public bool sprinkler = false; //fire in different directions
    public float rotateSpeed = 10f; //speed at which the cannon rotates to fire next direction
    public int rotationDuration = 100;
    int setDuration;
    bool increaseRot;

    public bool trackPlayer = false; //aim at the player

    public bool onlyFireWhenPlayerVisible = false;
    RaycastHit2D hit;
    bool canSeePlayer;

    GameObject player; //player to track
    float currentDistance;
    float maxDistance = 23f;

    // Start is called before the first frame update
    void Start()
    {
        asas = GameObject.FindGameObjectWithTag("UIcontrol").GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");
        setCooldown = fireCooldown;
        setDuration = rotationDuration * 2;

        if (Random.Range(0, 2) == 0)
            increaseRot = true;
        else
            increaseRot = false;
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Linecast(transform.position, player.transform.position);

        if (hit != false)
        {
            if (hit.collider.gameObject.tag == ("Player"))
                canSeePlayer = true;
            else
                canSeePlayer = false;
        }

        currentDistance = Vector2.Distance(transform.position, player.transform.position);

        if (currentDistance < maxDistance)
        {
            if (rotationDuration > 0)
                rotationDuration--;

            if (fireCooldown > 0)
                fireCooldown--;

            if (sprinkler) //fire in 3 directions
            {
                if (rotationDuration <= 0)
                {
                    if (increaseRot == true)
                        increaseRot = false;
                    else
                        increaseRot = true;

                    rotationDuration = setDuration;
                }
                
                if (increaseRot)
                    transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);
                else
                    transform.Rotate(-rotateSpeed * Time.deltaTime, 0, 0);
            }
            else if (trackPlayer) //track the player
            {
                transform.LookAt(player.transform);
            }
            
            if (fireCooldown <= 0 && !onlyFireWhenPlayerVisible)
            {
                fireCooldown = setCooldown;

                GameObject cb = Instantiate(projectile, spawnSpot.transform.position, new Quaternion(0, 0, 0, 0));

                //particles
                Instantiate(fireParticles, spawnSpot.transform.position, new Quaternion(0, 0, 0, 0));

                //sound
                asas.clip = fireSound;
                asas.PlayOneShot(fireSound, 0.3f);

                cb.GetComponent<Rigidbody2D>().velocity = transform.forward * cannonBallSpeed;
            }
            else if (fireCooldown <= 0 && onlyFireWhenPlayerVisible == true)
            {
                if (canSeePlayer == true)
                {
                    fireCooldown = setCooldown;

                    GameObject cb = Instantiate(projectile, spawnSpot.transform.position, new Quaternion(0, 0, 0, 0));

                    //particles
                    Instantiate(fireParticles, spawnSpot.transform.position, new Quaternion(0, 0, 0, 0));

                    //sound
                    asas.clip = fireSound;
                    asas.PlayOneShot(fireSound, 0.3f);

                    cb.GetComponent<Rigidbody2D>().velocity = transform.forward * cannonBallSpeed;
                }
            }
        }
    }
}
