using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuUI : MonoBehaviour
{
    AudioSource asasas;
    public AudioClip clickSound;
    public AudioClip ambience;

    public Camera cam;

    public Button startGame; //button that starts the game
    public Button controls; //button that shows the controls
    public Button tutorial; //tutorial button

    public Image fade; //image that will fade to black
    bool fadingIn;

    public GameObject character;

    bool startGameClicked = false; //if one of the buttons has been clicked
    bool controlsClicked = false;
    bool tutorialClicked = false;
    int slide = 0;

    public Image[] controlsBg;
    public TextMeshProUGUI controlsText;
    public TextMeshProUGUI topCtext;
    public TextMeshProUGUI bottomCtext;

    bool finishedMoving = false;

    private void Awake()
    {
        asasas = GetComponent<AudioSource>();

        controlsText.GetComponent<TextMeshProUGUI>();
        topCtext.GetComponent<TextMeshProUGUI>();
        bottomCtext.GetComponent<TextMeshProUGUI>();

        Color temp = fade.color;
        temp.a = 1f;
        fade.color = temp;

        fadingIn = true;

        startGame.onClick.AddListener(beginGame);
        controls.onClick.AddListener(showControls);
        tutorial.onClick.AddListener(startTutorial);

        startGame.interactable = false;
        controls.interactable = false;
        tutorial.interactable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void startTutorial()
    {
        startGame.interactable = false; //make buttons not clickable
        controls.interactable = false;
        tutorial.interactable = false;

        tutorialClicked = true;
    }

    void beginGame() //happens when startGame is clicked
    {
        startGame.interactable = false; //make buttons not clickable
        controls.interactable = false;
        tutorial.interactable = false;

        asasas.clip = ambience;
        asasas.PlayOneShot(ambience, 0.2f);

        startGameClicked = true;
    }

    void showControls() //when controls is clicked
    {
        startGame.interactable = false; //make buttons not clickable
        controls.interactable = false;
        tutorial.interactable = false;
        controlsClicked = true;
        asasas.clip = clickSound;
        asasas.PlayOneShot(clickSound, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingIn == true)
        {
            if (fade.color.a > 0f)
            {
                Color temp = fade.color;
                temp.a -= 0.01f;
                fade.color = temp;
            }
            
            if (startGame.transform.position.x < 0.4032f)
            {
                startGame.transform.position = new Vector3(startGame.transform.position.x + 1f, startGame.transform.position.y, 0);
                controls.transform.position = new Vector3(controls.transform.position.x + 1f, controls.transform.position.y, 0);
                tutorial.transform.position = new Vector3(tutorial.transform.position.x + 1f, tutorial.transform.position.y, 0);
            }
            
            if (fade.color.a <= 0.01f)
            {
                startGame.transform.position = new Vector3(0.4032f, startGame.transform.position.y, 0);
                controls.transform.position = new Vector3(0.4032f, controls.transform.position.y, 0);
                tutorial.transform.position = new Vector3(0.4032f, tutorial.transform.position.y, 0);

                startGame.interactable = true;
                controls.interactable = true;
                tutorial.interactable = true;
                fadingIn = false;
            }
        }

        if (tutorialClicked == true)
        {
            startGame.transform.position = new Vector3(startGame.transform.position.x, startGame.transform.position.y - 1f, 0);
            controls.transform.position = new Vector3(controls.transform.position.x, controls.transform.position.y - 1f, 0);
            tutorial.transform.position = new Vector3(tutorial.transform.position.x, tutorial.transform.position.y - 1f, 0);

            if (fade.color.a >= 1f)
            {
                SceneManager.LoadScene("TutorialScene");
            }

            if (fade.color.a < 1f)
            {
                Color temp = fade.color;
                temp.a += 0.01f;
                fade.color = temp;

                cam.orthographicSize = (cam.orthographicSize + 0.01f);
            }
        }

        if (startGameClicked == true && finishedMoving == true)
        {
            asasas.volume = asasas.volume -= 0.01f;

            if (fade.color.a >= 1f)
            {
                SceneManager.LoadScene("LevelScene");
            }

            if (fade.color.a < 1f)
            {
                Color temp = fade.color;
                temp.a += 0.01f;
                fade.color = temp;

                cam.orthographicSize = (cam.orthographicSize - 0.01f);
            }
        }
        else if (startGameClicked == true && finishedMoving == false)
        {
            startGame.transform.position = new Vector3(startGame.transform.position.x, startGame.transform.position.y + 1f, 0);
            controls.transform.position = new Vector3(controls.transform.position.x, controls.transform.position.y + 1f, 0);
            tutorial.transform.position = new Vector3(tutorial.transform.position.x, tutorial.transform.position.y + 1f, 0);

            //27.62
            if (character.transform.position.x < 34.6)
            {
                character.transform.position = new Vector2((character.transform.position.x + 0.1f),
                                                           character.transform.position.y);
            }
            else if (character.transform.position.x > 34.6)
            {
                character.transform.position = new Vector2(34.6f, character.transform.position.y);
                character.GetComponent<Animator>().SetBool("StandStill", true);
            }

            if (cam.transform.position.x < 37)
            {
                cam.transform.position = new Vector3((cam.transform.position.x + 0.15f), 
                                                      cam.transform.position.y,
                                                      cam.transform.position.z);
            }
            else if (cam.transform.position.x >= 37)
            {
                finishedMoving = true;
            }
        }

        if (controlsClicked == true)
        {
            for (int i = 0; i < controlsBg.Length; i++)
            {
                if (controlsBg[i].color.a < 1f)
                {
                    Color temp = controlsBg[i].color;
                    temp.a = 1f;
                    controlsBg[i].color = temp;
                }
            }

            bottomCtext.text = "Press Enter to Continue";

            /*
             0 - movement controls
             1 - card and open controls
             2 - card effects
             3 - mechanics
             */

            if (slide == 0)
            {
                topCtext.text = "Controls:";

                controlsText.text = "A/D or Left Arrow/Right Arrow: Move left/right\n\n" +
                                    "W or Up Arrow: Jump\n\n" +
                                    "O, NumPad 5, or Middle-Click: Open treasure chest\n";
            }
            else if (slide == 1)
            {
                topCtext.text = "Controls:";

                controlsText.text = "K, NumPad1, or Left-Click: Use card\n\n" +
                                    "L, NumPad3, or Right-Click: Discard current card to back of deck, or cancel current card effect\n";
            }
            else if (slide == 2)
            {
                topCtext.text = "Card Effects:";

                controlsText.text = "Clubs: Smashes most enemies and obstacles beneath or next to you. Can be canceled\n\n" +
                                    "Diamonds: Throw a card that can destroy most obstacles or enemies\n\n" +
                                    "Hearts: Temporary shield that protects you from smaller threats, CANNOT use other cards when active. Can be canceled\n\n" +
                                    "Spades: A trampoline or second jump\n";
            }
            else if (slide == 3)
            {
                topCtext.text = "Cards:";

                controlsText.text = "You begin with 52 cards, or 13 of each suit. " +
                                    "When you run out of cards, you will have access to unlimited random cards, but they may be weaker\n\n" +
                                    "You are free to die as many times as you want\n";
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                asasas.PlayOneShot(clickSound, 0.5f);

                if (slide < 3)
                    ++slide;
                else if (slide == 3)
                {
                    controlsClicked = false;

                    controlsText.text = "";
                    topCtext.text = "";
                    bottomCtext.text = "";

                    for (int i = 0; i < controlsBg.Length; i++)
                    {
                        Color temp = controlsBg[i].color;
                        temp.a = 0f;
                        controlsBg[i].color = temp;
                    }

                    startGame.interactable = true;
                    controls.interactable = true;
                    tutorial.interactable = true;

                    slide = 0;
                }
            }
        }
    }
}
