using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public Transform[] bPathNodes; //if needed
    Transform target;
    int targetIndex;

    public float bMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (bPathNodes.Length > 0)
        {
            target = bPathNodes[0];
            targetIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bPathNodes.Length < 1)
        {
            Debug.Log("A moving block's patrol pathNodes array is empty");
        }
        else
        {
            if (Vector2.Distance(transform.position, target.position) > 0.5f)
            {


                transform.position = Vector2.MoveTowards(transform.position,
                                                         target.position,
                                                         bMoveSpeed * Time.deltaTime);
            }
            else //change target
            {
                if (targetIndex == (bPathNodes.Length - 1))
                {
                    target = bPathNodes[0];
                    targetIndex = 0;
                }
                else
                {
                    ++targetIndex;
                    target = bPathNodes[targetIndex];
                }
            }
        }
    }
}
