using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cover : MonoBehaviour { 
    public List<GameObject> coverPoints;
    public GameObject goPrefab;
    public int numPoints;
    private string myShape;

	// Use this for initialization
    
	void Start () {
        coverPoints = new List<GameObject>();
        goPrefab = Resources.Load("Prefabs/Go") as GameObject;
        myShape = GetComponent<MeshFilter>().name;
        //Debug.Log(myShape);
        CreateCover();
	}
	
    void CreateCover()
    {
        switch(myShape)
        {
            case "Cube":
                //Debug.Log("creating cube cover");
                break;
            case "Cylinder":
                CylinderCover();
                //Debug.Log("creating cylinder cover");
                break;
            default:
                //Debug.Log("primitive not accounted for");
                break;
        }
    }

    void CubeCover()
    {
        //to be completed
    }

    void CylinderCover()
    {
        //note: only works with circular cylinders (scale.x == scale.y)
        float radiusX = transform.localScale.x / 2;
        float radiusZ = transform.localScale.z / 2;
        float angleStep = 360/numPoints;
        float angle = 0;0
        NavMeshHit navHit;

        for(int i = 0; i < numPoints; ++i)
        {
            Vector3 pos = transform.position;            
 
            pos.x = pos.x + radiusX * Mathf.Sin(angle * Mathf.Deg2Rad); 
            pos.z = pos.z + radiusZ * Mathf.Cos(angle * Mathf.Deg2Rad);

            if(NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
            {
                pos = navHit.position;
            }
            //spawn a waypoint at the calculated position
            GameObject coverPoint = Instantiate(goPrefab, pos, Quaternion.identity);
            coverPoint.transform.parent = transform;
            //add it to the current list
            coverPoints.Add(coverPoint);
            angle += angleStep;
        }     
    }
   
}
