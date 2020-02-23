using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrackenDetection : MonoBehaviour
{
    public bool playerInRange;
    PlayerController target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerInRange = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) //player is in detection area
    {
        if (collision.gameObject.tag == ("Player") && target.dead == false)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //player leaves detection area
    {
        if (collision.gameObject.tag == ("Player"))
        {
            playerInRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target.dead)
            playerInRange = false;
    }
}
