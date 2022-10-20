using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //Rigidbody player;
    public event System.Action OnGoalReached;
    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (OnGoalReached != null)
            {
                OnGoalReached();
                GameObject playerMovement = GameObject.FindGameObjectWithTag("Player");
                playerMovement.GetComponent<PlayerMovement>().enabled = false;
            }
        }
    }


}
