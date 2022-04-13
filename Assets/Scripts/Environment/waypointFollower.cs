using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//generic class for having the moving platforms.
public class waypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    [SerializeField] private float speed = 2f;
    public GameObject platform;

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
       //Basic move from point a to point b based on a trigger.
            platform.transform.position = Vector2.MoveTowards(platform.transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }


    //spear activating lever is intentional. Changes the trigger to activate the lever pull form player & spear.
    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if(collision.gameObject.layer == 7 || collision.gameObject.layer == 6)
        {
            currentWaypointIndex = 1;
        }
    }

}
