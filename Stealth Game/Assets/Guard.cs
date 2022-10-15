using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Guard : MonoBehaviour
{
    public Transform pathholder;

    public float speed;
    public float delay;
    //set the turn speed to 90 degrees/ sec
    public float turnSpeed = 90;

    public Light spotLight;
    public float viewDistance;

    public Rigidbody playerRb;
    public LayerMask ignoreMe;
    float viewAngle;

    bool playerSeen;
    public Color lightColor;

    void Start()
    {
        playerSeen = false;
        
        viewAngle = spotLight.spotAngle;
        Vector3[] points = new Vector3[pathholder.childCount];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = pathholder.GetChild(i).position;
            points[i].y = transform.position.y;
        }


     
       
            StartCoroutine(FollowPath(points));
        
        
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypoint = 1;

        Vector3 targetPosition = waypoints[targetWaypoint];
        //Vector3 targetDirection = (targetPosition - transform.position).normalized;
        transform.LookAt(targetPosition);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (transform.position == targetPosition)
            {
                targetWaypoint = (1 + targetWaypoint) % waypoints.Length;
                targetPosition = waypoints[targetWaypoint];
                
                yield return new WaitForSeconds(delay);
                yield return StartCoroutine(TurnToFace(targetPosition));
                
                
            }
            yield return null;
        }

    }
    
    void  FixedUpdate()
    {
            float playerDistance = (transform.position - playerRb.transform.position).magnitude;
            Vector3 playerDirection = (playerRb.transform.position - transform.position).normalized;
            if (Mathf.Abs(playerDistance) < viewDistance)
            {
            RaycastHit hit;
            Debug.DrawRay(transform.position, playerDirection * playerDistance, Color.yellow);

            if (Physics.Raycast(transform.position, playerDirection, out hit, playerDistance))
 
                {
                if (hit.collider.tag != "obstacle")
                {
                    float playerAngle = Mathf.Atan2(playerDirection.x, playerDirection.z) * Mathf.Rad2Deg;
                    if (playerAngle < 0)
                    {
                        playerAngle += 360;
                    }
                    float guardAngleMax = transform.eulerAngles.y + (viewAngle / 2);
                    float guardAngleMin = transform.eulerAngles.y - (viewAngle / 2);
                    //Debug.Log("player Angle" + playerAngle);
                    //Debug.Log("Angle Max" + guardAngleMax);
                    //Debug.Log("Angle Min" + guardAngleMin);


                    if (playerAngle < guardAngleMax && playerAngle > guardAngleMin && playerSeen == false)
                    {
                        playerSeen = true;
                        spotLight.color = Color.red;
                    }
                    
                }
                }

            }
    }

       


    //create new coroutine for the turn function
    IEnumerator TurnToFace (Vector3 lookTarget)
    {
        //calculate the angle that the gaurd needs to be facing the lookTarget
        //if you have a direction, you can use trig to find the corresponding angle.
        Vector3 directionToLookTarget = (lookTarget - transform.position).normalized;
        //arc tan 2 returns radians so multiply by 180/pi to get degrees.
        float targetAngle = 90 - MathF.Atan2(directionToLookTarget.z,directionToLookTarget.x) * Mathf.Rad2Deg;


        //make a while loop that ends after the difference between the angles is less than 0.05..... using 0 is bad because of small impercision we may never get there.
        while (MathF.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            //use eulerangles?
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            //Debug.Log("transform angle" + transform.eulerAngles);
            transform.eulerAngles = Vector3.up * angle;
            
            yield return null;
        }

    }
   
    

   

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathholder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathholder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }


}
