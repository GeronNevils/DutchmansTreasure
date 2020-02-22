using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    Rigidbody2D rbb;
    bool die;
    int dieTimer = 100;


    private void Awake()
    {
        rbb = GetComponent<Rigidbody2D>();
        die = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (die == true && dieTimer > 0)
            dieTimer--;

        if (die == true && dieTimer == 0)
            Destroy(gameObject);
    }

    public void chainDestroyed()
    {
        die = true;
        rbb.constraints = RigidbodyConstraints2D.None;

        float xSpd = (float)Random.Range(-5, 6);
        float ySpd = (float)Random.Range(-5, 6);

        rbb.velocity = new Vector2(xSpd, ySpd);
    }
}
