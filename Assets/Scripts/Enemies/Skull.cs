using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    public GameObject particleDe;

    bool isToast = false;
    int deSpawnTimer = 200;

    Rigidbody2D arbys;

    Transform target; //the player to chase
    Vector2 startingPos; //spot to return to if player is dead

    public float skullMoveSpeed = 1f;
    public bool isImperviousToCards = false;

    float maxDistance = 27;
    float distanceToPlayer;

    bool canSeePlayer; //if the skull can see the player

    Vector2 currentTarget; //current target spot

    SpriteRenderer ssr;

    RaycastHit2D hit;

    private void Awake()
    {
        arbys = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        ssr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.gameObject.tag == ("Diamond") && !isImperviousToCards) ||
            (col.gameObject.tag == ("Club") && !isImperviousToCards) ||
            col.gameObject.tag == ("Cannonball"))
        {
            Instantiate(particleDe, transform.position, new Quaternion(0, 0, 0, 0));
            isToast = true;
            Destroy(gameObject.GetComponent<Collider2D>());

            arbys.gravityScale = 1;
            float xSpd = (float)Random.Range(-5, 6);
            float ySpd = (float)Random.Range(0, 3);

            arbys.velocity = new Vector2(xSpd, ySpd);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isToast) //not dead
        {
            //RaycastHit2D hit;

            hit = Physics2D.Linecast(transform.position, target.position);

            if (hit != false)
            {
                if (hit.collider.gameObject.tag == ("Player"))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }

            distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (distanceToPlayer < maxDistance && target.GetComponent<PlayerController>().dead == false)
            {
                if (!canSeePlayer)
                    currentTarget = startingPos;
                else if (canSeePlayer)
                    currentTarget = target.position;
            }
            else //player is too far away
            {
                currentTarget = startingPos;
            }

            if (currentTarget.x > transform.position.x) //face towards target
                ssr.flipX = true;
            else
                ssr.flipX = false;

            transform.position = Vector2.MoveTowards(transform.position, currentTarget, skullMoveSpeed * Time.deltaTime);
        }
        else //dead
        {
            transform.Rotate(0, 0, 750 * Time.deltaTime); //spin

            if (deSpawnTimer > 0)
                deSpawnTimer--;
            else if (deSpawnTimer <= 0)
                Destroy(gameObject);
        }
    }
}
