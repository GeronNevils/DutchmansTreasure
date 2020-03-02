using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public GameObject boat;
    bool moveBoat = false;

    public GameObject playerImg;
    Rigidbody2D movePlayer;

    public Image faderBlack;
    bool fadeIn = true;

    public Image faderBlue;
    bool fadeOut = false;

    private void Awake()
    {
        movePlayer = playerImg.GetComponent<Rigidbody2D>();
        playerImg.GetComponent<SpriteRenderer>().flipX = true;

        Color temp = faderBlack.color;
        temp.a = 1f;
        faderBlack.color = temp;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn == true)
        {
            if (faderBlack.color.a > 0f)
            {
                Color temp = faderBlack.color;
                temp.a -= 0.01f;
                faderBlack.color = temp;
            }

            if (faderBlack.color.a < 0.01f)
            {
                movePlayer.velocity = new Vector2(-7f, 12f);
                fadeIn = false;
            }
        }
        else if (fadeIn == false && fadeOut == false)
        {
            if (movePlayer.velocity.x == 0f && moveBoat == false)
            {
                playerImg.GetComponent<Animator>().SetBool("FinishedMoving", true);
                playerImg.GetComponent<SpriteRenderer>().flipX = false;
                moveBoat = true;
            }
            else if (moveBoat == true)
            {
                if (boat.transform.position.y < 23f)
                {
                    boat.transform.position = new Vector2(boat.transform.position.x,
                                                         (boat.transform.position.y + 0.1f));
                }
                else
                    fadeOut = true;
            }
        }
        else if (fadeOut == true)
        {
            if (faderBlue.color.a < 1f)
            {
                Color temp = faderBlue.color;
                temp.a += 0.01f;
                faderBlue.color = temp;
            }
        }
    }
}
