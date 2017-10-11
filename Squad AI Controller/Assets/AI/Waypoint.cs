using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waypoint : MonoBehaviour {
    public float score;
    public bool taken = false;

    public void SetTaken(bool setting)
    {
        taken = setting;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Squad_Member")
        {
            taken = false;
        }
    }
    // Use this for initialization
    void Start () {
        
        NavMeshHit navHit;
        if(NavMesh.FindClosestEdge(transform.position, out navHit, NavMesh.AllAreas))
        {
            transform.position = navHit.position;
        }
        
	}
}
