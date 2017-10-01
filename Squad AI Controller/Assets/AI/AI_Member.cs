using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AI_Member : MonoBehaviour {
    public Vector3 goal;
    private NavMeshAgent agent;
    //private NavMeshPath path;

	// Use this for initialization
	void Start () {
        goal = transform.position;
        agent = GetComponent<NavMeshAgent>();
        //path = new NavMeshPath();
	}
	
    public void MoveTo(Vector3 dest)
    {
        //Check that the target is reasonably reachable
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(dest, out navHit, 2.0f, NavMesh.AllAreas))
        {
            goal = navHit.position;
            agent.destination = goal;
        }
        else
        {
            Debug.Log("not reachable");
        }
    }

	// Update is called once per frame
	void Update () {
        Debug.DrawLine(transform.position, goal, Color.red);
	}
}