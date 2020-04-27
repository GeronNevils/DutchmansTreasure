using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DrawStat : MonoBehaviour
{
    bool starDrawing = false;
    public bool isDoneDrawing = false;

    public TextMeshProUGUI congratsText;
    public TextMeshProUGUI continueText;

    Image c;
    int extraCounter = 0;
    int drawTimer = 10;
    bool okayEnough = false;

    StatTracker tracky;
    bool showYaMoves = false;
    int phase = 0;

    public GameObject moveableParent;
    public Image roomOutline;
    public Image cardClub;
    public Image cardDiamond;
    public Image cardHeart;
    public Image cardSpade;
    public Image dedIcon;

    AudioSource asdf;
    public AudioClip showCards;
    public AudioClip showDeaths;
    public AudioClip fanfareButNotReally;

    private void Awake()
    {
        congratsText.GetComponent<TextMeshProUGUI>();
        continueText.GetComponent<TextMeshProUGUI>();

        tracky = GameObject.FindGameObjectWithTag("StatTracker").GetComponent<StatTracker>();

        asdf = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        congratsText.text = "";
        continueText.text = "";
    }

    public void okGo()
    {
        starDrawing = true;
    }

    public void setDrawTimer()
    {
        drawTimer = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (starDrawing == true)
        {
            if (drawTimer > 0)
                drawTimer--;

            if (phase == 0)
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

                    congratsText.text = "Nice Work!";

                    continueText.text = "Press Enter to Continue";

                    okayEnough = true;
                }

                isDoneDrawing = true;
            }
        }
    }
}
