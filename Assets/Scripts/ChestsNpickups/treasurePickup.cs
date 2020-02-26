using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treasurePickup : MonoBehaviour
{
    public int treasureValue = 5;
    StatTracker ssr;

    private void Awake()
    {
        ssr = GameObject.FindGameObjectWithTag("UIcontrol").GetComponent<StatTracker>();
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
            ssr.treasureCollected += treasureValue;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
