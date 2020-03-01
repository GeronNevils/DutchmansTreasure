using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tchest : MonoBehaviour
{
    SpriteRenderer srr;
    Animator openOrNot;

    public GameObject pickupParticles;
    public GameObject[] lootContained; //pickups in the chest
    bool open;

    private void Awake()
    {
        srr = GetComponent<SpriteRenderer>();
        openOrNot = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        openOrNot.SetBool("open", false);
        open = false;
    }

    private void OnTriggerStay2D(Collider2D co)
    {
        //if the player is touching the treasure chest and hits one of the open keys
        if (co.gameObject.tag == ("Player") && open == false &&
            (Input.GetKeyDown("o") || Input.GetKeyDown(KeyCode.Keypad5)))
        {
            Instantiate(pickupParticles, transform.position, new Quaternion(0, 0, 0, 0));
            dropLoot();
            open = true;
        }
    }

    void dropLoot()
    {
        openOrNot.SetBool("open", true);

        for (int i = 0; i < lootContained.Length; i++)
        {
            Instantiate(lootContained[i], transform.position, new Quaternion(0, 0, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
