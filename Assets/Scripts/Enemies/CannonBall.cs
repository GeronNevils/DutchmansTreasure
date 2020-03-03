using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    AudioSource sdsd;
    public AudioClip boomSound;

    int lifeSpan = 1000;
    public GameObject explodeParticles;

    // Start is called before the first frame update
    void Start()
    {
        sdsd = GameObject.FindGameObjectWithTag("UIcontrol").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && collision.gameObject.tag != "Obstacle") //hit solid object
        {
            //Particles
            Instantiate(explodeParticles, transform.position, new Quaternion(0, 0, 0, 0));

            //Sound
            sdsd.clip = boomSound;
            sdsd.PlayOneShot(boomSound, 0.3f);

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
