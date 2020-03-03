using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainParent : MonoBehaviour
{
    public GameObject[] chains;

    public GameObject particleD;

    AudioSource asa;
    public AudioClip ede;

    // Start is called before the first frame update
    void Start()
    {
        asa = GameObject.FindGameObjectWithTag("UIcontrol").GetComponent<AudioSource>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Club") || collision.gameObject.tag == ("Diamond") || collision.gameObject.tag == ("Cannonball"))
        {
            for (int i = 0; i < chains.Length; i++)
            {
                chains[i].GetComponent<Chain>().chainDestroyed();
            }
            Instantiate(particleD, collision.gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            asa.clip = ede;
            asa.PlayOneShot(ede, 0.4f);

            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
