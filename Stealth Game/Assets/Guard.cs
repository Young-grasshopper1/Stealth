using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Guard : MonoBehaviour
{
    public Transform pathholder;

    public float speed;
    public float delay;

    IEnumerator currentCoroutine;

    void Start()
    {
        Vector3[] points = new Vector3[pathholder.childCount];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = pathholder.GetChild(i).position;
        }

        StartCoroutine(FollowPath(points, delay));
    }

    IEnumerator FollowPath (Vector3[] waypoints, float delay)
    {
        foreach (Vector3 point in waypoints)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = move(point, speed);
            StartCoroutine(currentCoroutine);
            yield return new WaitForSeconds(delay);
        }

        currentCoroutine = move(waypoints[0], speed);
        StartCoroutine(currentCoroutine);
        yield return new WaitForSeconds (delay);
        
    }


    IEnumerator move(Vector3 destination, float speed)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
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
