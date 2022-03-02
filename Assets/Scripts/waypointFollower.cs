using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class waypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    [SerializeField] private float speed = 2f;
    private GameObject lever;

    private void Update()
    {
        /*
         * for moving back & forth
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }*/
        activate();
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }

    public void activate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentWaypointIndex = 1;
            
        }
    }
}