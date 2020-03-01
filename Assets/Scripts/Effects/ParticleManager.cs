using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    //int lifeSpan = 600;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<ParticleSystem>().isStopped == true)
            Destroy(gameObject);

        //if (lifeSpan > 0)
            //lifeSpan--;
        //else if (lifeSpan <= 0)
            //Destroy(gameObject);
    }
}
