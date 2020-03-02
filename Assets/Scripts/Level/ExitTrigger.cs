using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public bool gameFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        gameFinished = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameFinished = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
