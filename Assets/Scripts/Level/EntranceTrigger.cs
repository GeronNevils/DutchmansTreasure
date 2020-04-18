using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger : MonoBehaviour
{
    public bool currentRoomEntrance = false;
    int cooldown = 0;

    bool soundPlayed = false;
    AudioSource audsou;
    public AudioClip transSound;

    // Start is called before the first frame update
    void Start()
    {
        audsou = GameObject.FindGameObjectWithTag("UIcontrol").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && 
            currentRoomEntrance == false &&
            collision.gameObject.GetComponent<PlayerController>().dead == false)
        {
            setEntrance();
        }
    }

    void setEntrance()
    {
        cooldown = 100;
        currentRoomEntrance = true;
        if (soundPlayed == false)
        {
            audsou.clip = transSound;
            audsou.PlayOneShot(transSound, 1f);
            soundPlayed = true;
        }
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
