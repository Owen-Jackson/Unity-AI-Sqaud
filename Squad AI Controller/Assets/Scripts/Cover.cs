using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cover : MonoBehaviour
{
    public List<GameObject> leftPoints;
    public List<GameObject> rightPoints;
    public List<GameObject> topPoints;
    public List<GameObject> bottomPoints;
    //public GameObject goPrefab;
    // Use this for initialization

    void Awake()
    {
        //goPrefab = Resources.Load("Prefabs/Go") as GameObject;
        leftPoints = new List<GameObject>();
        rightPoints = new List<GameObject>();
        topPoints = new List<GameObject>();
        bottomPoints = new List<GameObject>();
        //GeneratePoints();
    }

    /*
    void GeneratePoints()
    {
        Vector3 right = transform.right;
        Vector3 forw = transform.forward;
        Vector3 min = transform.position - (right * (transform.localScale.x / 2)) - (forw * (transform.localScale.z / 2)) - right / 2 - forw / 2;
        Vector3 max = transform.position + (right * (transform.localScale.x / 2)) + (forw * (transform.localScale.z / 2)) + right / 2 + forw / 2;
        Vector3 bottomLeft = min;
        Vector3 topRight = max;
        Vector3 topLeft = min + (forw * (transform.localScale.z)) + forw;
        Vector3 bottomRight = max - (forw * (transform.localScale.z)) - forw;
        Debug.DrawLine(min, max, Color.cyan, 10f);
        Vector3 pos = min;
        NavMeshHit navHit;
        RaycastHit rayHit;

        //loop along the edges, raycasting inwards at set intervals
        //bottomleft to topleft
        for (int i = 0; i < transform.localScale.z + 2; i++)
        {
            if (i % 2 == 1)
            {
                pos = bottomLeft;
                pos += forw * i;
                if (Physics.Raycast(pos, right, out rayHit, 2f))
                {
                    if (rayHit.collider.gameObject.tag == "Cover")
                    {
                        if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                        {
                            Debug.Log("creating left coverpoint in loop: " + i);
                            //if (NavMesh.FindClosestEdge(navHit.position, out navHit, NavMesh.AllAreas))
                            {
                                GameObject go = Instantiate(goPrefab, pos, transform.rotation);
                                go.transform.position = navHit.position;
                                go.transform.parent = transform;
                                leftPoints.Add(go);
                            }
                        }
                        else
                        {
                            Debug.Log("point: " + i + " not made");
                        }
                    }
                    else
                    {
                        Debug.Log("point: " + i + " not made");
                    }
                }
            }
        }

        //bottomleft to bottomright
        for (int i = 0; i < transform.localScale.x + 2; i++)
        {
            if (i % 2 == 1)
            {
                pos = bottomLeft;
                pos += right * i;
                if (Physics.Raycast(pos, forw, out rayHit, 2f))
                {
                    //Debug.Log("hit ray on bottom");
                    if (rayHit.collider.gameObject.tag == "Cover")
                    {
                        //Debug.Log("tag check pass");
                        if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                        {
                            Debug.Log("creating bottom coverpoint in loop: " + i);
                            //if (NavMesh.FindClosestEdge(navHit.position, out navHit, NavMesh.AllAreas))
                            {
                                GameObject go = Instantiate(goPrefab, pos, transform.rotation);
                                go.transform.position = navHit.position;
                                go.transform.parent = transform;
                                bottomPoints.Add(go);
                            }
                        }
                    }
                }
            }
        }

        //topleft to topright
        for (int i = 0; i < transform.localScale.x + 2; i++)
        {
            if (i % 2 == 1)
            {
                pos = topLeft;
                pos += right * i;
                if (Physics.Raycast(pos, -forw, out rayHit, 2f))
                {
                    if (rayHit.collider.gameObject.tag == "Cover")
                    {

                        if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                        {
                            Debug.Log("creating top coverpoint in loop: " + i);
                            //if (NavMesh.FindClosestEdge(navHit.position, out navHit, NavMesh.AllAreas))
                            {
                                GameObject go = Instantiate(goPrefab, pos, transform.rotation);
                                go.transform.position = navHit.position;
                                go.transform.parent = transform;
                                topPoints.Add(go);
                            }
                        }
                    }
                }
            }
        }

        //bottomright to topright
        for (int i = 0; i < transform.localScale.z + 2; i++)
        {
            if (i % 2 == 1)
            {
                pos = bottomRight;
                pos += forw * i;
                if (Physics.Raycast(pos, -right, out rayHit, 2f))
                {
                    if (rayHit.collider.gameObject.tag == "Cover")
                    {
                        if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                        {
                            Debug.Log("creating right coverpoint in loop: " + i);
                            //if (NavMesh.FindClosestEdge(navHit.position, out navHit, NavMesh.AllAreas))
                            {
                                GameObject go = Instantiate(goPrefab, pos, transform.rotation);
                                go.transform.position = navHit.position;
                                go.transform.parent = transform;
                                rightPoints.Add(go);
                            }
                        }
                    }
                }
            }
        }
        //CreateMantlePoints();
        //Debug.Log("points generated");
    }
    */

    public void CreateMantlePoints()
    {
        //Note: this method only works on box shapes
        if (transform.localScale.y <= 1.25)
        {
            //go through each list and raycast with parallel waypoints, 
            //if the distance is short enough then they can be vaulted between
            //Left and Right
            for (int i = 0; i < leftPoints.Count; i++)
            {
                Debug.DrawLine(leftPoints[i].transform.position, rightPoints[i].transform.position, Color.yellow, 5f);
                if (Vector3.Distance(leftPoints[i].transform.position, rightPoints[i].transform.position) < 4f)
                {
                    
                    leftPoints[i].GetComponent<Waypoint>().SetOffMeshLink(rightPoints[i]);
                }
            }

            //Top and Bottom
            for (int i = 0; i < bottomPoints.Count; i++)
            {
                Debug.DrawLine(bottomPoints[i].transform.position, topPoints[i].transform.position, Color.yellow, 5f);
                if (Vector3.Distance(bottomPoints[i].transform.position, topPoints[i].transform.position) < 4f)
                {
                    bottomPoints[i].GetComponent<Waypoint>().SetOffMeshLink(topPoints[i]);
                }
            }
        }
    }
}