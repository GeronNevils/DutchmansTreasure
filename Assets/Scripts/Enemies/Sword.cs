using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    SpriteRenderer sr;

    public Sprite[] swords; //we have multiple sword images

    public bool followPath = false; //follow a path
    public bool spinAround = false; //rotate around a point

    public float moveSpeed;

    public Transform[] pathNodes; //if needed
    Transform target;
    int targetIndex;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        int img = Random.Range(0, swords.Length);

        sr.sprite = swords[img];
    }

    // Start is called before the first frame update
    void Start()
    {
        if (followPath && pathNodes.Length > 0)
        {
            target = pathNodes[0];
            targetIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 500 * Time.deltaTime);

        if (spinAround == true)
        {
            if (pathNodes.Length < 1)
            {
                Debug.Log("A sword's spin around pathNodes array is empty");
            }
            else
            {
                transform.RotateAround(pathNodes[0].transform.position, new Vector3(0, 0, 1), moveSpeed * Time.deltaTime);
            }
        }

        if (followPath == true)
        {
            if (pathNodes.Length < 1)
            {
                Debug.Log("A sword's patrol pathNodes array is empty");
            }
            else
            {
                if (Vector2.Distance(transform.position, target.position) > 0.5f)
                {
                    transform.position = Vector2.MoveTowards(transform.position,
                                                             target.position,
                                                             moveSpeed * Time.deltaTime);
                }
                else //change target
                {
                    if (targetIndex == (pathNodes.Length - 1))
                    {
                        target = pathNodes[0];
                        targetIndex = 0;
                    }
                    else
                    {
                        ++targetIndex;
                        target = pathNodes[targetIndex];
                    }
                }
            }
        }
    }
}
