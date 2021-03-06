﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kracken : MonoBehaviour
{
    public GameObject detectionArea; //Spot that detects player
    public GameObject moveTowards; //spot to move to for attacking
    public float attackSpeed = 5f;

    bool aggressed = false;

    Vector3 orgPosition;

    GameObject player; //target

    Animator animate;
    KrackenDetection detector;

    private void Awake()
    {
        animate = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        detector = detectionArea.GetComponent<KrackenDetection>();
        orgPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detector.playerInRange) //player in range
            aggressed = true;

        if (aggressed) //Attack
        {
            //move and attack
            animate.SetBool("playerClose", true);

            transform.position = Vector2.MoveTowards(transform.position, moveTowards.transform.position, attackSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, moveTowards.transform.position) < 0.1f)
            {
                aggressed = false;
            }
        }
        else //Retreat
        {
            //go back into hiding
            animate.SetBool("playerClose", false);

            transform.position = Vector2.MoveTowards(transform.position, orgPosition, (attackSpeed / 4f) * Time.deltaTime);
        }
    }
}
