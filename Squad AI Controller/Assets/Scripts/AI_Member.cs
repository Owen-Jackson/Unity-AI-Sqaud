using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AI_Member : MonoBehaviour {
    public GameObject player;
    public Squad_Controller myController;
    public float maxDistanceFromPlayer = 5;
    public List<GameObject> nearbyPoints;
    public List<GameObject> nearbyEnemies;
    public GameObject currentWaypoint;
    public List<float> scoresList;
    public int currentIndex;
    public NavMeshAgent agent;
    public float distFromCentre = 0;
    private List<GameObject> waypoints = null;
    private bool commanded = false; //is the unit currently commanded?
    private float commandTimer = 0; //time since the last command was issued. resets the AI when the player stop issuing commands
    private float autoTimer = 0;    //countdown to when they next move on their own

    // Use this for initialization
    void Start() {
        myController = transform.parent.GetComponent<Squad_Controller>();
        nearbyEnemies = new List<GameObject>();
        scoresList = new List<float>();
        agent = GetComponent<NavMeshAgent>();
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
        dest.GetComponent<Waypoint>().owner = this.gameObject;
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

    public void MoveCommand(List<GameObject> points)
    {
        autoTimer = 0;
        waypoints = points;
        currentWaypoint = points[0];
        currentIndex = 0;
        currentWaypoint.GetComponent<Waypoint>().SetTaken(true);
        currentWaypoint.GetComponent<Waypoint>().owner = this.gameObject;
        commanded = true;
        commandTimer = 0;
    }

    //Used when checking for nearby points to move to
    private void EvaluateNearbyPoints()
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

                if (gameObject.layer == LayerMask.NameToLayer("Player_Ally"))
                {
                    scoresList[count] += waypoint.GetComponent<Waypoint>().allyScore;
                }
                else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    scoresList[count] += waypoint.GetComponent<Waypoint>().enemyScore;
                }

                //General Threat Check
                count++;
            }
        }
    }
    
    public void MoveToNextBest()
    {
        EvaluateNearbyPoints();
        //if commanded search by nearest to command spot
        if (commanded)
        {
            MoveTo(waypoints[currentIndex + 1]);
            currentIndex++;
        }
        else
        {
            int newDest = 0;
            for (int i = 0; i < nearbyPoints.Count; i++)
            {
                if (!nearbyPoints[i].GetComponent<Waypoint>().taken)
                {
                    if(scoresList[i] > scoresList[newDest])
                    {
                        newDest = i;
                    }
                }
            }
            MoveTo(nearbyPoints[newDest]);
            currentIndex = newDest;
        }
    }

    float DistanceFromDest()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, currentWaypoint.transform.position, NavMesh.AllAreas, path);
        float distance = 0;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
        }
        return distance;
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
        if (distance < 10f)
        {
            score = 1 - (distance / 10);
        }
        return score;
    }

    //add a score to the waypoint if it is out of sight from enemies
    float LineOfSightEvaluation(GameObject waypoint)
    {
        float score = 0;
        if (myController.knownEnemies.Count > 0)
        {
            foreach (GameObject enemy in myController.knownEnemies)
            {
                if (Physics.Linecast(waypoint.transform.position, enemy.transform.position, 1<<9))
                {
                    score += 5;
                }
            }
        }
        return score;
    }

	// Update is called once per frame
	void Update () {
        Debug.DrawLine(transform.position, agent.destination, Color.red);
        //set commanded back to false after timer or if destination is reached
        if (commanded)
        {
            commandTimer += Time.deltaTime;
            if(commandTimer > 10f)
            {
                commanded = false;
                commandTimer = 0;
                MoveToNextBest();
            }
            else if(DistanceFromDest() < 1f)
            {
                commanded = false;
                commandTimer = 0;
                //Debug.Log("arrived");
            }
        }
        else
        {
            autoTimer += Time.deltaTime;
            if(autoTimer > 5f)
            {
                autoTimer = 0;
                MoveToNextBest();
            }
        }

        if (currentWaypoint)
        {
            //if my waypoint is taken, go to the next nearest. used to avoid multiple NPCs moving to same waypoint
            if(currentWaypoint.GetComponent<Waypoint>().owner != this.gameObject)
            {
                MoveToNextBest();                           
            }
            
            //target = currentWaypoint.transform.position;
            //used for dynamic obstacles - checks if waypoint's transform has changed and updates destination to its new position
            if (currentWaypoint.transform.hasChanged)
            {
                agent.SetDestination(currentWaypoint.transform.position);
                currentWaypoint.transform.hasChanged = false;
            }
        }
	}
}