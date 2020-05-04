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
    int setCardToRoom = 1;

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

    public void setTutorialCards()
    {
        deck.Clear();

        switch (setCardToRoom)
        {
            case 1: case 2: case 7: //give all 4 cards
                for (int j = 0; j < suits.Length; j++)
                {
                    Card d = new Card(suits[j], 1);
                    deck.Add(d);
                }
                break;
            case 3: //club
                Card c = new Card(suits[0], 1);
                deck.Add(c);
                break;
            case 4: //diamond
                Card b = new Card(suits[1], 1);
                deck.Add(b);
                break;
            case 5: //heart
                Card a = new Card(suits[2], 1);
                deck.Add(a);
                break;
            case 6: //spade
                Card f = new Card(suits[3], 1);
                deck.Add(f);
                break;
            case 8: //none (joker)
                break;
        }

        setCardToRoom++;
    }

    public void discard()
    {
        if (deck.Count > 0 && !controlFreeze.freeze)
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

                if (!isTutorial)
                    deck.RemoveAt(0); //get rid of the used card
                else
                {
                    deck.Add(deck[0]);
                    deck.RemoveAt(0);
                }
            }
            else if ((Input.GetKeyDown("k") || 
                      Input.GetKeyDown(KeyCode.Keypad1) ||
                      Input.GetMouseButtonDown(0)) && deck.Count < 1 && playerCon.cardActive == false && currentCooldown <= 0) //use a joker
            { 
                playerCon.joker();
                currentCooldown = jokerCooldown;
            }
        }
    }
}
