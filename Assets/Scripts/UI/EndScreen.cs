using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI congratsText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI continueText;

    Image c;
    int extraCounter = 0;
    int drawTimer = 10;
    bool okayEnough = false;

    StatTracker tracky;
    bool showYaMoves = false;
    int phase = -1;
    public GameObject dropShip;
    public GameObject moveableParent;
    public Image roomOutline;
    public Image cardClub;
    public Image cardDiamond;
    public Image cardHeart;
    public Image cardSpade;
    public Image dedIcon;

    AudioSource asdf;
    public AudioClip bAmbience;
    public AudioClip showOutlines;
    public AudioClip showCards;
    public AudioClip showDeaths;
    public AudioClip fanfareButNotReally;

    public GameObject boat;
    bool moveBoat = false;

    public GameObject playerImg;
    Rigidbody2D movePlayer;

    public Image faderBlack;
    bool fadeIn = true;

    public SpriteRenderer faderBlue;
    bool fadeOut = false;

    private void Awake()
    {
        congratsText.GetComponent<TextMeshProUGUI>();
        statsText.GetComponent<TextMeshProUGUI>();
        continueText.GetComponent<TextMeshProUGUI>();

        tracky = GameObject.FindGameObjectWithTag("StatTracker").GetComponent<StatTracker>();

        asdf = GetComponent<AudioSource>();

        movePlayer = playerImg.GetComponent<Rigidbody2D>();
        playerImg.GetComponent<SpriteRenderer>().flipX = true;

        Color temp = faderBlack.color;
        temp.a = 1f;
        faderBlack.color = temp;
    }

    // Start is called before the first frame update
    void Start()
    {
        congratsText.text = "";
        statsText.text = "";
        continueText.text = "";

        asdf.clip = bAmbience;
        asdf.PlayOneShot(bAmbience, 0.2f);
    }

    public void setDrawTimer()
    {
        drawTimer = 10;
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
        else if (fadeOut == true && showYaMoves == false)
        {
            asdf.volume = asdf.volume -= 0.01f;

            if (faderBlue.color.a < 1f)
            {
                Color temp = faderBlue.color;
                temp.a += 0.01f;
                faderBlue.color = temp;
            }
            else if (faderBlue.color.a >= 1f)
            {
                asdf.Stop();
                asdf.volume = 1f;
                showYaMoves = true;
            }
        }
        else if (showYaMoves)
        {
            if (drawTimer > 0)
                drawTimer--;

            if (phase == -1)
            {
                if (dropShip.transform.position.y > 28.5f)
                {
                    dropShip.transform.position = new Vector3(dropShip.transform.position.x,
                                                              dropShip.transform.position.y - 2f, 0);
                }
                else if (dropShip.transform.position.y <= 28.5f)
                {
                    asdf.clip = showOutlines;
                    asdf.PlayOneShot(showOutlines, 0.3f);
                    phase++;
                }
            }
            else if (phase == 0)
            {
                //draw room outlines:
                if (drawTimer <= 0)
                {
                    for (int i = 0; i < tracky.roomCoordinates.Count; i++)
                    {
                        c = Instantiate(roomOutline);
                        c.transform.SetParent(moveableParent.transform, false);
                        c.transform.localPosition = new Vector3(tracky.roomCoordinates[i].x, tracky.roomCoordinates[i].y, 0);
                        c.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    }

                    phase++;
                    setDrawTimer();
                }
            }
            else if (phase == 1)
            {
                //draw cards
                if (drawTimer <= 0)
                {
                    if (extraCounter < tracky.cerds.Count)
                    {
                        switch (tracky.cerds[extraCounter].cSuit)
                        {
                            case "Clubs":
                                c = Instantiate(cardClub);
                                break;
                            case "Diamonds":
                                c = Instantiate(cardDiamond);
                                break;
                            case "Hearts":
                                c = Instantiate(cardHeart);
                                break;
                            case "Spades":
                                c = Instantiate(cardSpade);
                                break;
                        }
                        c.transform.SetParent(moveableParent.transform, false);
                        c.transform.localPosition = new Vector3(tracky.cerds[extraCounter].cX, tracky.cerds[extraCounter].cY, 0);
                        c.transform.localRotation = new Quaternion(0, 0, 0, 0);

                        asdf.clip = showCards;
                        asdf.PlayOneShot(showCards, 0.7f);

                        extraCounter++;
                        setDrawTimer();
                    }
                    else
                    {
                        phase++;
                        extraCounter = 0;
                        setDrawTimer();
                    }
                }
            }
            else if (phase == 2)
            {
                if (drawTimer <= 0)
                {
                    //draw death icons
                    if (extraCounter < tracky.deds.Count)
                    {
                        c = Instantiate(dedIcon);
                        c.transform.SetParent(moveableParent.transform, false);
                        c.transform.localPosition = new Vector3(tracky.deds[extraCounter].x, tracky.deds[extraCounter].y, 0);
                        c.transform.localRotation = new Quaternion(0, 0, 0, 0);

                        asdf.clip = showDeaths;
                        asdf.PlayOneShot(showDeaths, 0.7f);

                        extraCounter++;
                        setDrawTimer();
                    }
                    else
                    {
                        phase++;
                        extraCounter = 0;
                        setDrawTimer();
                    }
                }
            }
            else if (phase == 3)
            {
                if (drawTimer <= 0 && okayEnough == false)
                {
                    asdf.clip = fanfareButNotReally;
                    asdf.PlayOneShot(fanfareButNotReally, 0.7f);

                    congratsText.text = "You've Done It!";
                    statsText.text = "Treasure Collected: " + tracky.treasureCollected + "g\n" +
                                     "        Cards Used: " + tracky.cardsUsed + "\n" +
                                     "      Total Deaths: " + tracky.numOfDeaths;

                    continueText.text = "Press Enter to Continue";

                    okayEnough = true;
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    phase++;
                }
            }
            else if (phase == 4)
            {
                if (faderBlack.color.a < 1f)
                {
                    Color temp = faderBlack.color;
                    temp.a += 0.01f;
                    faderBlack.color = temp;
                }

                if (faderBlack.color.a >= 1f)
                {
                    SceneManager.LoadScene("MenuScene");
                }
            }
        }
    }
}
