using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AI_Member : MonoBehaviour {
    public GameObject player;
    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
    public void MoveTo(Waypoint dest)
    {
        agent.destination = dest.transform.position;
        dest.SetTaken(true);
    }

    public void MoveTo(Vector3 dest)
    {
        agent.destination = dest;
    }

	// Update is called once per frame
	void Update () {
        Debug.DrawLine(transform.position, agent.destination, Color.red);
	}
}