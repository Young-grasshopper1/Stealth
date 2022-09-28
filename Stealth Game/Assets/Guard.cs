using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Guard : MonoBehaviour
{
    public Transform pathholder;

    public float speed;
    void Start()
    {
        Vector3[] waypoints = new Vector3[pathholder.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathholder.GetChild(i).position;
        }
        for (int i = 0; i < waypoints.Length; i++)
        {
            StartCoroutine(move(waypoints[i]));
        }
        
    }

    // edit
    IEnumerator move(Vector3 waypoints )
    {
        while (transform.position != waypoints)
        {
            Vector3 direction = (waypoints - transform.position).normalized;
            Vector3 velocity = direction * speed;
            transform.position += velocity;
        }

        yield return StartCoroutine(move(waypoints));
    }

    void OnDrawGizmos()
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
