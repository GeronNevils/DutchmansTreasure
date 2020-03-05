using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    public static StatTracker instance;

    public int treasureCollected;
    public int numOfDeaths;
    public int cardsUsed;
    //int totalTime;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
