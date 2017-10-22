using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Squad_Controller : MonoBehaviour {
    public AI_Member[] squad;
    public List<GameObject> knownEnemies;
    public Vector3 averageSquadPos;
    public float minSeparation = 2;
    public float maxSeparation = 20;

    // Use this for initialization
    void Start () {
        //get squad members to control
        squad = GetComponentsInChildren<AI_Member>();
        if(squad.Length > 0)
        {
            Vector3 avg = Vector3.zero;
            foreach(AI_Member member in squad)
            {
                avg += member.transform.position;
                member.myController = this;
            }
            averageSquadPos = avg / squad.Length;
        }
        knownEnemies = new List<GameObject>();
	}

    public void MoveSquad(Vector3 dest)
    {
        List<GameObject> points = new List<GameObject>();
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(dest, out navHit, 2.0f, NavMesh.AllAreas))
        {
            dest = navHit.position;
            //find waypoints near target
            Collider[] hitColliders = Physics.OverlapSphere(dest, 5.0f, 1<<8);
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                if (hitColliders[i].tag == "Waypoint")
                {
                    //if (!Physics.Linecast(hitColliders[i].transform.position, navHit.position))
                   // {
                        points.Add(hitColliders[i].gameObject);
                        //Debug.Log(points.Count);
                    //}
                }
            }
            switch(points.Count)
            {
                case 0:
                    SendWithoutWaypoints(dest);
                    break;
                case 1:
                    SendOneWaypoint(points[0]);
                    break;
                default:
                    SendMultipleWaypoints(dest, points);
                    break;
            }
        }
    }
    
    void SendMultipleWaypoints(Vector3 dest, List<GameObject> points)
    {
        //sort points to get the ones closest to the command location
        
        points.Sort(delegate (GameObject a, GameObject b)
        {
            return Vector3.Distance(dest, a.transform.position).CompareTo(Vector3.Distance(dest, b.transform.position));
        });

        //move each member in the squad to their points
        int count = 0;
        foreach (AI_Member member in squad)
        {
            if(count > points.Count-1)
            {
                break;
            }
            member.MoveToNextBestFromGiven(points);
            /*if (points[count].GetComponent<Waypoint>().taken == false)
            {
                member.MoveTo(points[count]);
            }*/
            ++count;
        }
    }

    void SendOneWaypoint(GameObject waypoint)
    {
        AI_Member agentToSend = squad[0];
        foreach (AI_Member member in squad)
        {
            //move the agent furthest from the waypoint
            if (Vector3.Distance(waypoint.transform.position, member.transform.position) > Vector3.Distance(waypoint.transform.position, agentToSend.transform.position))
            { 
                agentToSend = member;
            }
        }
        Debug.Log("send to one");
        agentToSend.GetComponent<NavMeshAgent>().destination = waypoint.transform.position;
    }

    void SendWithoutWaypoints(Vector3 dest)
    {
        int count = 0;
        foreach (AI_Member member in squad)
        {
            member.MoveTo(dest);     
            ++count;
        }
    }

    void CheckKnownEnemies()
    {
        /*
        //check for enemies to remove
        if (knownEnemies.Count > 0)
        {
            for(int i = 0; i < knownEnemies.Count; i++)
            {
                int seenCount = 0;
                foreach (AI_Member member in squad)
                {
                    if (member.nearbyEnemies.Contains(knownEnemies[i]))
                    {
                        seenCount++;
                    }
                }
                if (seenCount == 0)
                {
                    knownEnemies.Remove(knownEnemies[i]);
                }
            }
        }
        */
        //add newly seen enemies
        foreach(AI_Member member in squad)
        {
            if(member.nearbyEnemies.Count > 0)
            {
                foreach (GameObject enemy in member.nearbyEnemies)
                {
                    if(!knownEnemies.Contains(enemy))
                    {
                        knownEnemies.Add(enemy);
                    }
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
        Vector3 avg = Vector3.zero;
        CheckKnownEnemies();

        foreach (AI_Member member in squad)
        {
            avg += member.transform.position;
        }
        averageSquadPos = avg / squad.Length;
        foreach (AI_Member member in squad)
        {
            member.distFromCentre = Vector3.Distance(member.transform.position, avg);
        }
    }
}
