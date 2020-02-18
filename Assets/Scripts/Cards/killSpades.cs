using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killSpades : MonoBehaviour
{
    int killTime = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (killTime == 0)
            Destroy(gameObject);

        if (killTime > 0)
            killTime--;
    }
}
