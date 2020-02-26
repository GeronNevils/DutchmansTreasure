using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mimicChest : MonoBehaviour
{
    Animator an; //animation controller
    GameObject player; //player to track
    PlayerController pl; //for calling player to disappear

    float aggroDistance = 1.5f; //how close the player has to be to attack
    public int mimicAttackSpeed = 40;
    int attackTimer;
    bool aggro;
    int hungerTimer = 0;

    private void Awake()
    {
        an = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        pl = player.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = mimicAttackSpeed;
        an.SetBool("playerNear", false);
        an.SetBool("playerCaught", false);
        aggro = false;
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag == ("Player") && attackTimer <= 0 && pl.dead == false && pl.shieldsUp == false) //touching player
        {
            an.SetBool("playerCaught", true);
            an.SetBool("playerNear", false);
            aggro = false;
            attackTimer = mimicAttackSpeed;
            hungerTimer = 200;

            pl.caughtByMimic();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hungerTimer > 0)
            hungerTimer--;

        if (hungerTimer <= 0)
        {
            an.SetBool("playerCaught", false);
            if (Vector2.Distance(transform.position, player.transform.position) < aggroDistance && pl.dead == false)
            {
                aggro = true;
                an.SetBool("playerNear", true);
            }
            else //player is outside of aggro range
            {
                aggro = false;
                an.SetBool("playerNear", false);
                attackTimer = mimicAttackSpeed;
            }

            if (aggro == true && attackTimer > 0)
                attackTimer--;
        }
    }
}
