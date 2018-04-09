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