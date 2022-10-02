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
    public float lerpDuration;



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
        Vector3 targetDirection = (targetPosition - transform.position).normalized;
        transform.rotation *= Quaternion.FromToRotation(transform.forward, targetDirection);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            if (transform.position == targetPosition)
            {
                targetWaypoint = (1 + targetWaypoint) % waypoints.Length;
                targetPosition = waypoints[targetWaypoint];
                
                yield return new WaitForSeconds(delay);
                targetDirection = (targetPosition - transform.position).normalized;
                Debug.Log("target direction" + targetDirection);
                Vector3 axis = Vector3.Cross(transform.forward, targetDirection);
                Debug.Log("axis" + axis);
                float t = 0f;


                while (t < lerpDuration)
                {
                    float delta = Mathf.Lerp(0, Vector3.Angle(transform.forward, targetDirection), t/ lerpDuration);
                    transform.rotation *= Quaternion.AngleAxis(delta, axis);
                    t += Time.deltaTime;
                    yield return null;
                   
                }
                
            }
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
