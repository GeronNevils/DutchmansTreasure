using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainParent : MonoBehaviour
{
    public GameObject[] chains;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Club") || collision.gameObject.tag == ("Diamond") || collision.gameObject.tag == ("Cannonball"))
        {
            for (int i = 0; i < chains.Length; i++)
            {
                chains[i].GetComponent<Chain>().chainDestroyed();
            }

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
