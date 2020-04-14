using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card //Contains a suit and value
{
    public Card(string s, int n)
    {
        suit = s;
        num = n;
    }

    public string suit { get; }
    public int num { get; }
}

public class cardController : MonoBehaviour
{
    PlayerController playerCon;

    GameUI controlFreeze;

    public bool isTutorial = false;

    public List<Card> deck = new List<Card>();
    string[] suits = { "Clubs", "Diamonds", "Hearts", "Spades" };

    int jokerCooldown = 60;
    int currentCooldown;

    void Awake()
    {
        playerCon = GetComponent<PlayerController>();
        controlFreeze = GameObject.FindGameObjectWithTag("UIcontrol").GetComponent<GameUI>();

        for (int i = 1; i <= 13; i++)
        {
            for (int j = 0; j < suits.Length; j++)
            {
                Card c = new Card(suits[j], i);
                deck.Add(c);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void discard()
    {
        if (deck.Count > 0)
        {
            deck.Add(deck[0]);
            deck.RemoveAt(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (deck.Count < 1 && currentCooldown > 0)
            currentCooldown--;

        if (playerCon.dead == false && !controlFreeze.freeze)
        {
            if ((Input.GetKeyDown("k") || 
                 Input.GetKeyDown(KeyCode.Keypad1) ||
                 Input.GetMouseButtonDown(0)) && deck.Count > 0 && playerCon.cardActive == false) //use a card
            {
                if (deck[0].suit == "Clubs")
                {
                    playerCon.club(true);
                }
                else if (deck[0].suit == "Diamonds")
                {
                    playerCon.diamond(true);
                }
                else if (deck[0].suit == "Hearts")
                {
                    playerCon.heart(true);
                }
                else if (deck[0].suit == "Spades")
                {
                    playerCon.spade(true);
                }

                deck.RemoveAt(0); //get rid of the used card
            }
            else if ((Input.GetKeyDown("k") || 
                      Input.GetKeyDown(KeyCode.Keypad1) ||
                      Input.GetMouseButtonDown(0)) && deck.Count < 1 && playerCon.cardActive == false && currentCooldown <= 0) //use a joker
            { //add NumPad 3 input
                playerCon.joker();
                currentCooldown = jokerCooldown;
            }
        }
    }
}
