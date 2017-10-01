using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Squad_Controller : MonoBehaviour {
    public AI_Member[] squad;
    public Vector3 averageSquadPos;
    public float minSeparation = 2;
    public float maxSeparation = 5;

     // Use this for initialization
    void Start () {
        //get squad members to control
        squad = FindObjectsOfType<AI_Member>();
        if(squad.Length > 0)
        {
            Vector3 avg = Vector3.zero;
            foreach(AI_Member member in squad)
            {
                avg += member.transform.position;
            }
            averageSquadPos = avg / squad.Length;
        }
	}

	public void MoveSquad(Vector3 dest)
    {
        //create an area of possible positions to move to
        List<Vector3> points = new List<Vector3>();
        NavMeshHit navHit;
        while(points.Count < squad.Length)
        {
            //get a random point within a circle
            Vector3 randDest = Random.insideUnitCircle * 5;
            //add the offset from the origin
            randDest += dest;
            //check the position is on the navmesh
            if(NavMesh.SamplePosition(randDest, out navHit , 2.0f, NavMesh.AllAreas))
            {
                points.Add(navHit.position);
            }
        }
        //move each member in the squad to their points
        int count = 0;
        foreach (AI_Member member in squad)
        {
            member.MoveTo(points[count]);
            ++count;
        }
    }

	// Update is called once per frame
	void Update () {
        Vector3 avg = Vector3.zero;
        foreach (AI_Member member in squad)
        {
            avg += member.transform.position;
        }
        averageSquadPos = avg / squad.Length;
    }
}
