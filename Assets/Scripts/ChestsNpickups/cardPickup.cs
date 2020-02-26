using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardPickup : MonoBehaviour
{
    cardController con;

    private void Awake()
    {
        con = GameObject.FindGameObjectWithTag("Player").GetComponent<cardController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            //particles
            //sound
            con.addCards();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
