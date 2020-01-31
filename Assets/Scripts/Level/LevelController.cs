using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Camera cam; //the camera to aim at the room
    public GameObject entrance; //The trigger object and respawn point for the room
    public GameObject blocker; //The object that turns solid to keep the player from backtracking

    EntranceTrigger check;

    void Awake()
    {
        check = entrance.GetComponent<EntranceTrigger>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (check.currentRoomEntrance == true)
        {
            //move camera
            cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(
                                                                                    transform.position.x,
                                                                                    transform.position.y,
                                                                                    -10f), 0.1f);
            //set blocker to be solid
            blocker.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
