﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    //Manages the hitboxes for the diamond and club cards

    GameObject player;
    Collider2D col;
    public bool isDiamond = false;
    public GameObject vanishParticles;
    int lifespan = 600; //how long the hitbox will stick around, mostly for culling leftover diamond cards

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(col, player.GetComponent<Collider2D>());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //diamond hit solid object
        if (collision.gameObject.layer == 8 && isDiamond == true && collision.gameObject.tag != "Diamond" && collision.gameObject.tag != "Obstacle")
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll; //keep them from floating around
            col.isTrigger = false; //make the card solid
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lifespan < 590)
            Physics2D.IgnoreCollision(col, player.GetComponent<Collider2D>(), false);

        if (lifespan > 0)
            lifespan--;
        else if (lifespan == 0)
        {
            Instantiate(vanishParticles, transform.position, new Quaternion(0, 0, 0, 0));
            Destroy(gameObject);
        }

        if (isDiamond) //make card go spinny
        {
            if (GetComponent<Rigidbody2D>().velocity.x != 0.0f)
            {
                transform.Rotate(0, 0, 250 * Time.deltaTime);
            }
        }
    }
}
