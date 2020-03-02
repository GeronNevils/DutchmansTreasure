using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public Camera cam;

    public Button startGame; //button that starts the game
    public Button controls; //button that shows the controls

    public Image fade; //image that will fade to black

    public GameObject character;

    bool startGameClicked = false; //if one of the buttons has been clicked
    bool controlsClicked = false;
    int slide = 0;

    public Image controlsBg;
    public TextMeshProUGUI controlsText;
    public TextMeshProUGUI topCtext;
    public TextMeshProUGUI bottomCtext;

    bool finishedMoving = false;

    private void Awake()
    {
        controlsText.GetComponent<TextMeshProUGUI>();
        topCtext.GetComponent<TextMeshProUGUI>();
        bottomCtext.GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startGame.onClick.AddListener(beginGame);
        controls.onClick.AddListener(showControls);

        startGame.interactable = true;
        controls.interactable = true;
    }

    void beginGame() //happens when startGame is clicked
    {
        startGame.interactable = false; //make buttons not clickable
        controls.interactable = false;

        startGameClicked = true;
    }

    void showControls() //when controls is clicked
    {
        startGame.interactable = false; //make buttons not clickable
        controls.interactable = false;
        controlsClicked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startGameClicked == true && finishedMoving == true)
        {
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
            if (controlsBg.color.a < 1f)
            {
                Color temp = controlsBg.color;
                temp.a = 1f;
                controlsBg.color = temp;

                bottomCtext.text = "Press Enter to Continue";
            }

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
                                    "O or NumPad 5: Open treasure chest\n";
            }
            else if (slide == 1)
            {
                topCtext.text = "Controls:";

                controlsText.text = "K or NumPad1: Use card\n\n" +
                                    "L or NumPad3: Discard current card/cancel current card effect\n\n";
            }
            else if (slide == 2)
            {
                topCtext.text = "Card Effects:";

                controlsText.text = "Clubs: Smashes most enemies and obstacles beneath or next to you\n\n" +
                                    "Diamonds: Throw a card that can destroy most obstacles or enemies\n\n" +
                                    "Hearts: Temporary shield that protects you from smaller threats\n\n" +
                                    "Spades: A trampoline or second jump\n";
            }
            else if (slide == 3)
            {
                topCtext.text = "Cards:";

                controlsText.text = "You begin with 52 cards, or 13 of each suit. " +
                                    "When you run out of cards, you will have access to unlimited random cards\n\n" +
                                    "You are free to die as many times as you want\n";
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (slide < 3)
                    ++slide;
                else if (slide == 3)
                {
                    controlsClicked = false;

                    controlsText.text = "";
                    topCtext.text = "";
                    bottomCtext.text = "";

                    Color temp = controlsBg.color;
                    temp.a = 0f;
                    controlsBg.color = temp;

                    startGame.interactable = true;
                    controls.interactable = true;
                }
            }
        }
    }
}
