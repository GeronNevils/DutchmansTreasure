using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLoc
{
    public DeathLoc(float xPos, float yPos)
    {
        x = xPos;
        y = yPos;
    }

    public float x { get; }
    public float y { get; }
}

public class CardLoc
{
    public CardLoc(float xx, float yy, string ss)
    {
        cX = xx;
        cY = yy;
        cSuit = ss;
    }

    public float cX { get; }
    public float cY { get; }
    public string cSuit { get; }
}

public class StatTracker : MonoBehaviour
{
    public static StatTracker instance;

    public int treasureCollected = 0;
    public int numOfDeaths = 0;
    public int cardsUsed = 0;
    //int totalTime;

    public List<DeathLoc> deds = new List<DeathLoc>();
    public List<CardLoc> cerds = new List<CardLoc>();

    public List<DeathLoc> roomCoordinates = new List<DeathLoc>();

    public void addDeath(float xP, float yP)
    {
        numOfDeaths++;
        deds.Add(new DeathLoc(xP, yP));
    }

    public void addUsedCard(float xc, float yc, string sc)
    {
        cardsUsed++;
        cerds.Add(new CardLoc(xc, yc, sc));
    }

    public void addRoomPos(float rx7, float ry)
    {
        DeathLoc dl = new DeathLoc(rx7, ry);
        if (!roomCoordinates.Contains(dl))
            roomCoordinates.Add(dl);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void cleanOut()
    {
        treasureCollected = 0;
        numOfDeaths = 0;
        cardsUsed = 0;

        deds.Clear();
        cerds.Clear();
        roomCoordinates.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
