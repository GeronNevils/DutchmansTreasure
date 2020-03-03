using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treasurePickup : MonoBehaviour
{
    AudioSource ecruoSoiduA;
    public AudioClip pickSound;

    int collisionTimer = 60;
    BoxCollider2D col;
    CircleCollider2D ccol;
    GameObject player;

    public GameObject pickupParticles;
    public int treasureValue = 5;
    StatTracker ssr;

    private void Awake()
    {
        ecruoSoiduA = GameObject.FindGameObjectWithTag("UIcontrol").GetComponent<AudioSource>();

        col = GetComponent<BoxCollider2D>();
        ccol = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        ssr = GameObject.FindGameObjectWithTag("UIcontrol").GetComponent<StatTracker>();
        Physics2D.IgnoreCollision(col, player.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(ccol, player.GetComponent<Collider2D>());
    }

    // Start is called before the first frame update
    void Start()
    {
        int spd = Random.Range(-3, 4);
        GetComponent<Rigidbody2D>().velocity = new Vector2((float)spd, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            //particles
            Instantiate(pickupParticles, transform.position, new Quaternion(0, 0, 0, 0));

            //sound
            ecruoSoiduA.clip = pickSound;
            ecruoSoiduA.PlayOneShot(pickSound, 1f);

            ssr.treasureCollected += treasureValue;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (collisionTimer > -1)
            collisionTimer--;

        if (collisionTimer == 0)
        {
            Physics2D.IgnoreCollision(col, player.GetComponent<Collider2D>(), false);
            Physics2D.IgnoreCollision(ccol, player.GetComponent<Collider2D>(), false);
        }
    }
}
