using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Camera cam;

    public Button startGame; //button that starts the game
    public Button controls; //button that shows the controls

    public Image fade; //image that will fade to black

    public GameObject character;

    bool startGameClicked = false; //if one of the buttons has been clicked
    bool controlsClicked = false;

    bool finishedMoving = false;

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
        //show a series of images with text explaining controls
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
            //do something here
            Debug.Log("show controls");

            controlsClicked = false;
        }
    }
}
