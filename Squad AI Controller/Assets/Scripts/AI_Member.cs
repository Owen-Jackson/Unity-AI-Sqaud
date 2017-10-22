using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AI_Member : MonoBehaviour {
    public GameObject player;
    public Squad_Controller myController;
    public float maxDistanceFromPlayer = 5;
    public List<GameObject> nearbyPoints;
    //public List<GameObject> knownEnemies;
    public GameObject currentWaypoint;
    public List<float> scoresList;
    public int currentIndex;
    public NavMeshAgent agent;
    public NavMeshObstacle obst;
    public float distFromCentre = 0;
    public Vector3 target;


    // Use this for initialization
    void Start() {
        nearbyPoints = new List<GameObject>();
        //knownEnemies = new List<GameObject>();
        scoresList = new List<float>();
        agent = GetComponent<NavMeshAgent>();
        obst = GetComponent<NavMeshObstacle>();
    }

    public IEnumerator ToggleAgentState()
    {
        if (agent && agent.enabled)
        {
            agent.enabled = false;

            yield return new WaitForEndOfFrame();

            if (obst && !obst.enabled)
            {
                obst.enabled = true;
            }
        }
        else if (obst && obst.enabled)
        {
            obst.enabled = false;

            yield return new WaitForEndOfFrame();

            if(agent && !agent.enabled)
            {
                agent.enabled = true;
                agent.SetDestination(currentWaypoint.transform.position);
            }
        }
    }

    public void MoveTo(GameObject dest)
    {
        if(currentWaypoint)
        {
            currentWaypoint.GetComponent<Waypoint>().SetTaken(false);
        }
        agent.destination = dest.transform.position;
        currentWaypoint = dest;
        dest.GetComponent<Waypoint>().SetTaken(true);
    }

    public void MoveTo(Vector3 dest)
    {
        if (currentWaypoint)
        {
            currentWaypoint.GetComponent<Waypoint>().SetTaken(false);
        }
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
                scoresList[count] += LOSEvaluation(waypoint);

                //General Threat Check

                count++;
            }
        }
    }

    void MoveToNextBest()
    {

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

    float LOSEvaluation(GameObject waypoint)
    {
        float score = 0;
        if (myController.knownEnemies.Count > 0)
        {
            foreach (GameObject enemy in myController.knownEnemies)
            {
                if (Physics.Linecast(waypoint.transform.position, enemy.transform.position))
                {
                    score += 1;
                }
                else
                {
                    Debug.DrawLine(waypoint.transform.position, enemy.transform.position, Color.magenta, 1.0f);
                }
            }
        }
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
        if (currentWaypoint)
        {
            target = currentWaypoint.transform.position;
            if (currentWaypoint.transform.hasChanged)
            {
                agent.SetDestination(currentWaypoint.transform.position);
                currentWaypoint.transform.hasChanged = false;
            }
        }
	}
}