using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //this script is for the visual cards to follow the player

    GameObject target;

    float yTarget;
    float zTarget;

    public bool visualCard = false; //if the object is a card being used for visual purpose only
    public bool fallingHitbox = false; //the object is a hitbox that needs to be below the player

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player"); //target player
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (visualCard == true) //the card needs to show in front of the player
        {
            zTarget = -1f; //show in front of the player
        }
        else //match player's z coordinate
        {
            zTarget = target.transform.position.z;
        }

        if (fallingHitbox == true) //the hitbox needs to be slightly below the player
        {
            yTarget = target.transform.position.y - 0.3f; //slightly lower
        }
        else //match player's y coordinate
        {
            yTarget = target.transform.position.y;
        }

        transform.position = new Vector3(target.transform.position.x,
                                          yTarget,
                                          zTarget);
    }
}
