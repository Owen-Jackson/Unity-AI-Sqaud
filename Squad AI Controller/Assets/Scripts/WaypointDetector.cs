using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDetector : MonoBehaviour {
    public AI_Member parent;
    public bool isPlayerAlly;

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
        if (isPlayerAlly)
        {
            if (other.tag == "Squad_Member" && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("enemy nearby");
                if (!parent.nearbyEnemies.Contains(other.gameObject))
                {
                    parent.nearbyEnemies.Add(other.gameObject);
                }
            }
        }
        else
        {
            if (other.tag == "Squad_Member" && other.gameObject.layer == LayerMask.NameToLayer("Player_Ally"))
            {
                Debug.Log("enemy nearby");
                if (!parent.nearbyEnemies.Contains(other.gameObject))
                {
                    parent.nearbyEnemies.Add(other.gameObject);
                }
            }
        }
    }

    //Remove waypoints from the nearby list
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Waypoint")
        {
            if (parent.nearbyPoints.Contains(other.gameObject))
            {
                int index = parent.nearbyPoints.IndexOf(other.gameObject);
                parent.nearbyPoints.RemoveAt(index);
                parent.scoresList.RemoveAt(index);
            }
        }
        if (isPlayerAlly)
        {
            if (other.tag == "Squad_Member" && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (parent.nearbyEnemies.Contains(other.gameObject))
                {
                    parent.nearbyEnemies.Remove(other.gameObject);
                }
            }
        }
        else
        {
            if (other.tag == "Squad_Member" && other.gameObject.layer == LayerMask.NameToLayer("Player_Ally"))
            {
                if (parent.nearbyEnemies.Contains(other.gameObject))
                {
                    parent.nearbyEnemies.Remove(other.gameObject);
                }
            }
        }
    }
}
