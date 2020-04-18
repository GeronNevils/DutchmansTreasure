using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBlock : MonoBehaviour
{
    Transform target; //the player to chase
    Vector2 startingPos; //spot to return to if player is dead

    public float blockMoveSpeed = 1f;

    float maxDistance = 27;
    float distanceToPlayer;

    bool canSeePlayer; //if the skull can see the player

    Vector2 currentTarget; //current target spot

    RaycastHit2D hit;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics2D.Linecast(transform.position, target.position);

        if (hit != false)
        {
            if (hit.collider.gameObject.tag == ("Player"))
                canSeePlayer = true;
            else
                canSeePlayer = false;
        }

        distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer < maxDistance && target.GetComponent<PlayerController>().dead == false)
        {
            if (!canSeePlayer)
                currentTarget = startingPos;
            else if (canSeePlayer)
                currentTarget = target.position;
        }
        else //player is too far away
        {
            currentTarget = startingPos;
        }

        transform.position = Vector2.MoveTowards(transform.position, currentTarget, blockMoveSpeed * Time.deltaTime);
    }
}
