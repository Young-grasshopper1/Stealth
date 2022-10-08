using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        if (input.magnitude > 0)
        {
            Vector3 velocity = input * speed * Time.deltaTime;

            transform.Translate(velocity, Space.World);
            transform.rotation = Quaternion.LookRotation(input);
        }

    }
}
