using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    public GameObject exitt;

    public Image currentCard;

    public Sprite[] cardPics;

    public TextMeshProUGUI deathNumber;
    public TextMeshProUGUI treasureNumber;

    cardController conC;

    StatTracker tracks;

    public TextMeshProUGUI cardsLeft;

    public bool freeze;
    bool quit;

    public Image fade;

    private void Awake()
    {
        conC = GameObject.FindGameObjectWithTag("Player").GetComponent<cardController>();
        tracks = GameObject.FindGameObjectWithTag("StatTracker").GetComponent<StatTracker>();

        cardsLeft.GetComponent<TextMeshProUGUI>();
        deathNumber.GetComponent<TextMeshProUGUI>();
        treasureNumber.GetComponent<TextMeshProUGUI>();

        freeze = true;
        quit = false;

        Color temp = fade.color;
        temp.a = 1f;
        fade.color = temp;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void unFreeze()
    {
        freeze = false;
    }

    // Update is called once per frame
    void Update()
    {
        deathNumber.text = "" + tracks.numOfDeaths;
        treasureNumber.text = "" + tracks.treasureCollected + "g";

        if (freeze == false && Input.GetKeyDown(KeyCode.Backspace)) //restart level
        {
            SceneManager.LoadScene("LevelScene");
        }

        if (freeze == false && Input.GetKeyDown(KeyCode.Escape)) //quit to menu
        {
            quit = true;
            freeze = true;
        }

        if (exitt.GetComponent<ExitTrigger>().gameFinished == true) //game finished
        {
            quit = true;
            freeze = true;
        }

        if (fade.color.a > 0f && quit == false)
        {
            Color temp = fade.color;
            temp.a -= 0.01f;
            fade.color = temp;
        }

        if (fade.color.a < 0.01f && freeze == true && quit == false)
            unFreeze();

        if (fade.color.a < 1f && quit == true)
        {
            Color temp = fade.color;
            temp.a += 0.01f;
            fade.color = temp;
        }

        if (fade.color.a > 0.99f && quit == true && exitt.GetComponent<ExitTrigger>().gameFinished == false)
            SceneManager.LoadScene("MenuScene");
        else if (fade.color.a > 0.99f && quit == true && exitt.GetComponent<ExitTrigger>().gameFinished == true)
            SceneManager.LoadScene("CutScene");

        cardsLeft.text = "" + conC.deck.Count;

        if (conC.deck.Count > 0)
        {
            switch (conC.deck[0].suit)
            {
                case "Clubs":
                    currentCard.sprite = cardPics[0];
                    break;
                case "Diamonds":
                    currentCard.sprite = cardPics[1];
                    break;
                case "Hearts":
                    currentCard.sprite = cardPics[2];
                    break;
                case "Spades":
                    currentCard.sprite = cardPics[3];
                    break;
                default:
                    Debug.Log("Improper card suit for image");
                    break;
            }
        }
        else
        {
            currentCard.sprite = cardPics[4];
        }
    }
}
