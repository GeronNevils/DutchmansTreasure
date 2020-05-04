using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    bool showLevel1Stats = false;

    public GameObject exitt;

    public Image currentCard;

    public Image[] pauseBg;

    public Sprite[] cardPics;

    public TextMeshProUGUI deathNumber;
    public TextMeshProUGUI treasureNumber;
    public TextMeshProUGUI pauseText;

    bool paused = false;
    public Button yesQuit;
    public Button noQuit;

    public int roomsInLevel = 13;

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
        pauseText.GetComponent<TextMeshProUGUI>();
        treasureNumber.GetComponent<TextMeshProUGUI>();

        yesQuit.onClick.AddListener(QuitToMenu);
        noQuit.onClick.AddListener(unPause);

        yesQuit.interactable = false;
        noQuit.interactable = false;

        freeze = true;
        quit = false;

        Color temp = fade.color;
        temp.a = 1f;
        fade.color = temp;

        for (int i = 0; i < pauseBg.Length; i++)
        {
            Color emp = pauseBg[i].color;
            emp.a = 0f;
            pauseBg[i].color = emp;
        }

        pauseText.text = "";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void QuitToMenu() //quit to menu, duh
    {
        yesQuit.interactable = false;
        noQuit.interactable = false;

        pauseText.text = "";

        for (int i = 0; i < pauseBg.Length; i++)
        {
            Color emp = pauseBg[i].color;
            emp.a = 0f;
            pauseBg[i].color = emp;
        }

        quit = true;
        freeze = true;
    }

    void setPause()
    {
        paused = true;
        freeze = true;

        for (int i = 0; i < pauseBg.Length; i++)
        {
            Color emp = pauseBg[i].color;
            emp.a = 1f;
            pauseBg[i].color = emp;
        }

        pauseText.text = "Quit to Menu?\n" + "Y   /   N";

        yesQuit.interactable = true;
        noQuit.interactable = true;
    }

    void unPause() 
    {
        yesQuit.interactable = false;
        noQuit.interactable = false;

        pauseText.text = "";

        for (int i = 0; i < pauseBg.Length; i++)
        {
            Color emp = pauseBg[i].color;
            emp.a = 0f;
            pauseBg[i].color = emp;
        }

        paused = false;
        freeze = false;
    }

    void unFreeze()
    {
        freeze = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (showLevel1Stats == true &&
            GameObject.FindGameObjectWithTag("level1stat").GetComponent<DrawStat>().isDoneDrawing == true &&
            Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("SequelScene");
        }

        deathNumber.text = "" + tracks.numOfDeaths;

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentRoom <= roomsInLevel)
        {
            treasureNumber.text = "" + tracks.treasureCollected + "g"
                + "            Room " + GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().currentRoom + "/" + roomsInLevel;
        }
        else
            treasureNumber.text = "" + tracks.treasureCollected + "g";

        if (freeze == false && Input.GetKeyDown(KeyCode.Backspace)) //restart level
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (freeze == false && paused == false && Input.GetKeyDown(KeyCode.Escape)) //pressed escape
        {
            setPause();
        }
        else if (paused == true && Input.GetKeyDown(KeyCode.Escape)) //unpause
        {
            unPause();
        }

        if (paused == true && Input.GetKeyDown(KeyCode.Y))
            QuitToMenu();
        else if (paused == true && Input.GetKeyDown(KeyCode.N))
            unPause();

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

        if (fade.color.a < 0.01f && freeze == true && quit == false && paused == false)
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
        {
            Scene currentScn = SceneManager.GetActiveScene();

            switch (currentScn.name)
            {
                case "LevelScene":
                    //SceneManager.LoadScene("SequelScene");
                    GameObject.FindGameObjectWithTag("level1stat").GetComponent<DrawStat>().okGo();
                    showLevel1Stats = true;
                    break;
                case "SequelScene":
                    SceneManager.LoadScene("CutScene");
                    break;
                case "TutorialScene":
                    SceneManager.LoadScene("MenuScene");
                    break;
            }
        }

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
