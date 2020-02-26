using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject projectile; //the cannonball

    public float cannonBallSpeed = 2f;

    public int fireCooldown = 100; //for stationary and targeting cannons
    int setCooldown; //holds the original cooldown value

    public bool sprinkler = false; //fire in different directions
    public float rotateSpeed = 1f; //speed at which the cannon rotates to fire next direction

    public bool trackPlayer = false; //aim at the player

    GameObject player; //player to track, if needed

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        setCooldown = fireCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireCooldown > 0)
            fireCooldown--;

        if (sprinkler) //fire in 3 directions
        {

        }
        else if (trackPlayer) //track the player
        {

        }
        else //fire in one direction only
        {
            if (fireCooldown <= 0)
            {
                fireCooldown = setCooldown;

                GameObject cb = Instantiate(projectile, transform.position, new Quaternion(0, 0, 0, 0));

                //particles
                //sound

                cb.GetComponent<Rigidbody2D>().velocity = transform.forward * cannonBallSpeed;
            }
        }
    }
}
