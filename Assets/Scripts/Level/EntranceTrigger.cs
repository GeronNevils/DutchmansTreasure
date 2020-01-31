using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger : MonoBehaviour
{
    public bool currentRoomEntrance = false;
    int cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !currentRoomEntrance)
        {
            setEntrance();
        }
    }

    void setEntrance()
    {
        cooldown = 100;
        currentRoomEntrance = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0)
            cooldown--;

        if (currentRoomEntrance == true && cooldown == 0)
            currentRoomEntrance = false;
    }
}
