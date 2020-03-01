using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    int lifeSpan = 1000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && collision.gameObject.tag != "Obstacle") //hit solid object
        {
            //Particles
            //Sound
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeSpan > 0)
            lifeSpan--;
        else if (lifeSpan <= 0)
            Destroy(gameObject);
    }
}
