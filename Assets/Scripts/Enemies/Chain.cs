using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    Rigidbody2D rbb;
    bool die;
    int dieTimer = 200;

    float spnSpd;

    private void Awake()
    {
        rbb = GetComponent<Rigidbody2D>();
        die = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        int a = Random.Range(0, 2);
        if (a == 0)
            spnSpd = 250;
        else
            spnSpd = -250;
    }

    // Update is called once per frame
    void Update()
    {
        if (die == true && dieTimer > 0)
        {
            dieTimer--;
            transform.Rotate(0, 0, spnSpd * Time.deltaTime); //spin
        }

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
