using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AI_Member : MonoBehaviour {
    public GameObject player;
    public Squad_Controller myController;
    public float maxDistanceFromPlayer = 5;
    public List<GameObject> nearbyPoints;
    public GameObject currentWaypoint;
    public List<float> scoresList;
    public int currentIndex;
    public NavMeshAgent agent;
    public float distFromCentre = 0;


	// Use this for initialization
	void Start () {
        nearbyPoints = new List<GameObject>();
        scoresList = new List<float>();
        agent = GetComponent<NavMeshAgent>();
	}
	
    public void MoveTo(GameObject dest)
    {
        agent.destination = dest.transform.position;
        currentWaypoint = dest;
        dest.GetComponent<Waypoint>().SetTaken(true);
    }

    public void MoveTo(Vector3 dest)
    {
        currentWaypoint = null;
        agent.destination = dest;
    }

    //Used when checking for nearby points to move to
    void EvaluateNearbyPoints()
    {
        if (nearbyPoints.Count > 0)
        {
            int count = 0;
            foreach (GameObject waypoint in nearbyPoints)
            {
                //reset score before evaluating
                scoresList[count] = 0;
                //Distance Check
                scoresList[count] += DistanceEvaluation(waypoint);
                //Enemy LOS check
                //General Threat Check
                count++;
            }
        }
    }

    float DistanceEvaluation(GameObject waypoint)
    {
        float score = 0;
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, waypoint.transform.position, NavMesh.AllAreas, path);
        float distance = 0;
        for(int i = 0; i < path.corners.Length-1; i++)
        {
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }
        score = distance;
        return score;
    }

	// Update is called once per frame
	void Update () {
        Debug.DrawLine(transform.position, agent.destination, Color.red);
        EvaluateNearbyPoints();
        currentIndex = nearbyPoints.IndexOf(currentWaypoint);
        if(scoresList.Count > 0)
        {
            int bestInd = 0;
            for(int i = 0; i < scoresList.Count-1;i++)
            {
                if(scoresList[i+1] > scoresList[i])
                {
                    bestInd = i+1;
                }
            }
        }
        //agent.SetDestination(currentWaypoint.transform.position);
        //if(distFromCentre > myController.maxSeparation)
        //{
        //
        //}
	}

    //Add waypoints to nearby list
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint")
        {
            if (!nearbyPoints.Contains(other.gameObject))
            {
                nearbyPoints.Add(other.gameObject);
                scoresList.Add(0);
            }
        }
    }

    //Remove waypoints from the nearby list
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Waypoint")
        {
            if (nearbyPoints.Contains(other.gameObject))
            {
                int index = nearbyPoints.IndexOf(other.gameObject);
                nearbyPoints.RemoveAt(index);
                scoresList.RemoveAt(index);
            }
        }
    }
}