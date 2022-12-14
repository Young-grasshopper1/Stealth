using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float smoothTurnTime = 0.1f;
    public float turnSpeed;
    
    bool disabled;
    float smoothInputMagnitude;
    float smoothVelocity;
    float angle;
    Vector3 velocity;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //this surbsrices the disable method to the OnPlayerFound Event. If the player object gets destroyed it iwll be nice to remove the method from the player (scene is reloaded)
        Guard.OnPlayerFound += Disabled;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = Vector3.zero;
        if (!disabled)
        {
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        }
        float inputMagnitude = input.magnitude;
        //calculate rotation based on vector

        smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude,ref smoothVelocity, smoothTurnTime);

        float targetAngle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * Time.deltaTime * inputMagnitude);

        velocity = transform.forward * speed * smoothInputMagnitude;

        //transform.eulerAngles = Vector3.up * angle;
        //Vector3 velocity = input * speed * Time.deltaTime;
        

        //transform.Translate(transform.forward * speed * Time.deltaTime * smoothInputMagnitude, Space.World);
    }

    void Disabled()
    {
        disabled = true;
    }
    void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }

    void OnDestroy()
    {
        Guard.OnPlayerFound -= Disabled;
    }


}
