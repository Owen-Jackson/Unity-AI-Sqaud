using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class CoverCreate : MonoBehaviour {
    public Vector3 GridSize = Vector3.zero;
    public float detail = 1;
    public List<GameObject> levelObjects;
    public List<GameObject> coverPoints;
    public List<GameObject> finalPoints;
    public GameObject level;
    public Vector3 origin;
    private GameObject goPrefab;

    // Use this for initialization
    void Start () {
        goPrefab = Resources.Load("Prefabs/Go") as GameObject;
        coverPoints = new List<GameObject>();
        //origin = level.GetComponent<Collider>().bounds.min;
        GetLevel();
        points2();
        //CreateCover();
	}

    void GetLevel()
    {
        levelObjects = new List<GameObject>();
        foreach(Transform child in level.GetComponentInChildren<Transform>())
        {
            if (child.CompareTag("Cover"))
            {
                levelObjects.Add(child.gameObject);
            }
          
        }
        Debug.Log(levelObjects.Count + " cover objects in level");
    }

    void CreateCover()
    {
        GeneratePoints();
        FilterPoints();
    }

    //2nd attempt at point generation
    void points2()
    {
        if (levelObjects.Count > 0)
        {
            for (int i = 0; i < levelObjects.Count; ++i)
            {
                GeneratePoints(levelObjects[i]);
                //GeneratePoints(levelObjects[i].GetComponent<Renderer>().bounds.min, levelObjects[i].GetComponent<Renderer>().bounds.max);
            }
        }
    }

    //create a set of points against all 'cover'
    void GeneratePoints()
    {
        Vector3 pos = origin;
        NavMeshHit navHit;
        
        for(int i = 0; i < GridSize.x; ++i)
        {
            //commented out y axis so that points only generate on 2D grid 
            //for (int j = 0; j < ScanGrid.y; j++)
            //{
                for (int k = 0; k < GridSize.z; ++k)
                {
                        //go back to the origin
                        pos = origin;
                        //search for nearest point of cover
                        pos = new Vector3(pos.x + i, 0, pos.z + k);
                        if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                        {
                            if (NavMesh.FindClosestEdge(pos, out navHit, NavMesh.AllAreas))
                            {
                                coverPoints.Add(Instantiate(goPrefab, navHit.position, Quaternion.identity));
                            }

                    }
                //}
            }
        }
        
        Debug.Log("points generated");
    }

    
    void GeneratePoints(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        Vector3 right = obj.transform.right;
        Vector3 forw = obj.transform.forward;
        Vector3 min = obj.transform.position -(right * (obj.transform.localScale.x / 2)) - (forw * (obj.transform.localScale.z/2)) -right/2 - forw/2;
        Vector3 max = col.bounds.max + right + forw;
        Vector3 pivot = min;        
        Debug.DrawLine(min, max, Color.cyan, 10f);
        Vector3 pos = min;
        NavMeshHit navHit;

        for (float i = 0; i < obj.transform.localScale.z + 2; i++)
        {
            //commented out y axis for now so that points only generate on 2D grid 
            //for (int j = 0; j < yDiff; j++)
            //{
            //Vector3 z = i * forw;
            for (float k = 0; k< obj.transform.localScale.x + 2;k++)
            {
                //Vector3 x = k * right;
                //go back to the origin                
                //create cover point
                pos = min;
                pos.x += k;
                pos.z += i;
                
                if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                {
                    if (NavMesh.FindClosestEdge(navHit.position, out navHit, NavMesh.AllAreas))
                    {
                        coverPoints.Add(Instantiate(goPrefab, pos, Quaternion.identity));
                        coverPoints[coverPoints.Count - 1].transform.RotateAround(pivot, obj.transform.up, obj.transform.rotation.eulerAngles.y);
                    }
                }
                //}
            }
        }
        
        //Debug.Log("points generated");
    }
    

    void GeneratePoints(Vector3 min, Vector3 max)
    {
        Debug.Log("min: " + min + " , max: " + max);
        Vector3 pos = min;
        float xDiff = Mathf.Floor(max.x - min.x);
        float yDiff = Mathf.Floor(max.y - min.y);
        float zDiff = Mathf.Floor(max.z - min.z);
        Debug.Log(xDiff + " , " + yDiff + " , " + zDiff);
        NavMeshHit navHit;
        for (float i = 0; i < xDiff*2; i ++)
        {
            //commented out y axis for now so that points only generate on 2D grid 
            //for (int j = 0; j < yDiff; j++)
            //{
            for (float k = 0; k < zDiff*2; k++)
            {
                //go back to the origin
                pos = min;
                //search for nearest point of cover
                pos = new Vector3(pos.x + i, 0, pos.z + k);
                if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                {
                    if (NavMesh.FindClosestEdge(navHit.position, out navHit, NavMesh.AllAreas))
                    {
                        coverPoints.Add(Instantiate(goPrefab, navHit.position, Quaternion.identity));
                    }
                }
                //}
            }
        }

        Debug.Log("points generated");
    }
    //filter out bad/unnecessary points
    void FilterPoints()
    {

    }
}
