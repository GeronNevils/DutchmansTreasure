using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    SpriteRenderer sr;

    public Sprite[] swords; //we have multiple sword images

    public bool followPath = false; //follow a path
    public bool spinAround = false; //rotate around a point

    public Transform[] pathNodes; //if needed

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        int img = Random.Range(0, swords.Length);

        sr.sprite = swords[img];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 500 * Time.deltaTime);
    }
}
