using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDetector : MonoBehaviour {
    public AI_Member parent;

    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<AI_Member>();
    }

    //Add waypoints to nearby list
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint")
        {
            Debug.Log("waypoint hit");
            if (!parent.nearbyPoints.Contains(other.gameObject))
            {
                parent.nearbyPoints.Add(other.gameObject);
                parent.scoresList.Add(0);
            }
        }
    }

    //Remove waypoints from the nearby list
    void OnTriggerExit(Collider other)
    {
        //Debug.Log("left waypoint");
        if (other.tag == "Waypoint")
        {
            if (parent.nearbyPoints.Contains(other.gameObject))
            {
                int index = parent.nearbyPoints.IndexOf(other.gameObject);
                parent.nearbyPoints.RemoveAt(index);
                parent.scoresList.RemoveAt(index);
            }
        }
    }
}
