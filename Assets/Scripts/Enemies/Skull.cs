using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    public Transform target; //the player to chase
    Vector2 startingPos; //spot to return to if player is dead

    Vector2 currentTarget; //current target spot

    SpriteRenderer ssr;

    private void Awake()
    {
        ssr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget.x > transform.position.x)
            ssr.flipX = true;
        else
            ssr.flipX = false;

        if (target.GetComponent<PlayerController>().dead == false) //player is alive
        {
            //chase player
            currentTarget = target.transform.position;
        }
        else if (target.GetComponent<PlayerController>().dead == true) //player is dead
        {
            //return to original place to avoid spawnkilling
            currentTarget = startingPos;
        }

        transform.position = Vector2.MoveTowards(transform.position, currentTarget, Time.deltaTime);
    }
}
