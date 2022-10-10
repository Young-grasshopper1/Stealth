using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UIElements;

public class Guard : MonoBehaviour
{
    public Transform pathholder;

    public float speed;
    public float delay;
    //set the turn speed to 90 degrees/ sec
    public float turnSpeed = 90;



    void Start()
    {
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

    //create new coroutine for the turn function
    IEnumerator TurnToFace (Vector3 lookTarget)
    {
        //calculate the angle that the gaurd needs to be facing the lookTarget
        //if you have a direction, you can use trig to find the corresponding angle.
        Vector3 directionToLookTarget = (lookTarget - transform.position).normalized;
        //arc tan 2 returns radians so multiply by 180/pi to get degrees.
        float targetAngle = 90 - MathF.Atan2(directionToLookTarget.z,directionToLookTarget.x) * Mathf.Rad2Deg;

  
  

        Debug.Log("target angle" + targetAngle);
        Debug.Log("delta" + Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle));
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
    }


}
