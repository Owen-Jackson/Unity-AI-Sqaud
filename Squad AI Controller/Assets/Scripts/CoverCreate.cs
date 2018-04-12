using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class CoverCreate : MonoBehaviour
{
    public float detail = 1;
    public List<GameObject> levelObjects;
    public List<GameObject> coverPoints;
    public GameObject level;
    public Vector3 origin;
    private GameObject goPrefab;

    // Use this for initialization
    void Start()
    {
        goPrefab = Resources.Load("Prefabs/Go") as GameObject;
        coverPoints = new List<GameObject>();
        GetLevel();
        GenerateCover();
    }

    void GetLevel()
    {
        levelObjects = new List<GameObject>();
        foreach (Transform child in level.GetComponentInChildren<Transform>())
        {
            if (child.CompareTag("Cover"))
            {
                levelObjects.Add(child.gameObject);
            }

        }
        //Debug.Log(levelObjects.Count + " cover objects in level");
    }

    //2nd attempt at point generation
    void GenerateCover()
    {
        if (levelObjects.Count > 0)
        {
            for (int i = 0; i < levelObjects.Count; ++i)
            {
                GeneratePoints(levelObjects[i]);
                //levelObjects[i].AddComponent<Cover>();
            }
        }
    }

    void GeneratePoints(GameObject obj)
    {
        obj.AddComponent<Cover>();
        Vector3 right = obj.transform.right;
        Vector3 forw = obj.transform.forward;
        Vector3 min = obj.transform.position - (right * (obj.transform.localScale.x / 2)) - (forw * (obj.transform.localScale.z / 2)) - right / 2 - forw / 2;
        Vector3 max = obj.transform.position + (right * (obj.transform.localScale.x / 2)) + (forw * (obj.transform.localScale.z / 2)) + right / 2 + forw / 2;
        Vector3 bottomLeft = min;
        Vector3 topLeft = min + (forw * (obj.transform.localScale.z)) + forw;
        Vector3 bottomRight = max - (forw * (obj.transform.localScale.z)) - forw;
        //Debug.DrawLine(min, max, Color.cyan, 10f);
        Vector3 pos = min;
        NavMeshHit navHit;
        RaycastHit rayHit;

        //loop along the edges, raycasting inwards at set intervals
        //bottomleft to topleft
        for (int i = 0; i < obj.transform.localScale.z + 2; i++)
        {
            if (i % 2 == 1)
            {
                pos = bottomLeft;
                pos += forw * i;
                if (Physics.Raycast(pos, right, out rayHit, 2f, 1 << 9))
                {
                    if (rayHit.collider.gameObject == obj)
                    {
                        if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                        {
                            if (Physics.OverlapSphere(navHit.position, 0.5f, 1 << LayerMask.NameToLayer("Waypoint")).Length == 0)
                            {
                                GameObject go = Instantiate(goPrefab, pos, obj.transform.rotation);
                                go.transform.position = navHit.position;
                                go.transform.parent = obj.transform;
                                obj.GetComponent<Cover>().leftPoints.Add(go);
                            }
                        }
                    }
                }
            }
        }

        //bottomleft to bottomright
        for (int i = 0; i < obj.transform.localScale.x + 2; i++)
        {
            if (i % 2 == 1)
            {
                pos = bottomLeft;
                pos += right * i;
                if (Physics.Raycast(pos, forw, out rayHit, 2f, 1 << 9))
                {
                    if (rayHit.collider.gameObject == obj)
                    {
                        if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                        {
                            if (Physics.OverlapSphere(navHit.position, 0.5f, 1 << LayerMask.NameToLayer("Waypoint")).Length == 0)
                            {
                                GameObject go = Instantiate(goPrefab, pos, obj.transform.rotation);
                                go.transform.position = navHit.position;
                                go.transform.parent = obj.transform;
                                obj.GetComponent<Cover>().bottomPoints.Add(go);
                            }
                        }
                    }
                }
            }
        }

        //topleft to topright
        for (int i = 0; i < obj.transform.localScale.x + 2; i++)
        {
            if (i % 2 == 1)
            {
                pos = topLeft;
                pos += right * i;
                if (Physics.Raycast(pos, -forw, out rayHit, 2f, 1 << 9))
                {
                    if (rayHit.collider.gameObject == obj)
                    {
                        if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                        {
                            if (Physics.OverlapSphere(navHit.position, 0.5f, 1 << LayerMask.NameToLayer("Waypoint")).Length == 0)
                            {
                                GameObject go = Instantiate(goPrefab, pos, obj.transform.rotation);
                                go.transform.position = navHit.position;
                                go.transform.parent = obj.transform;
                                obj.GetComponent<Cover>().topPoints.Add(go);
                            }
                        }
                    }
                }
            }
        }

        //bottomright to topright
        for (int i = 0; i < obj.transform.localScale.z + 2; i++)
        {
            if (i % 2 == 1)
            {
                pos = bottomRight;
                pos += forw * i;
                if (Physics.Raycast(pos, -right, out rayHit, 2f, 1 << 9))
                {
                    if (rayHit.collider.gameObject == obj)
                    {
                        if (NavMesh.SamplePosition(pos, out navHit, 2.0f, NavMesh.AllAreas))
                        {
                            if (Physics.OverlapSphere(navHit.position, 0.5f, 1 << LayerMask.NameToLayer("Waypoint")).Length == 0)
                            {
                                GameObject go = Instantiate(goPrefab, pos, obj.transform.rotation);
                                go.transform.position = navHit.position;
                                go.transform.parent = obj.transform;
                                obj.GetComponent<Cover>().rightPoints.Add(go);
                            }
                        }
                    }
                }
            }
        }
        //obj.GetComponent<Cover>().CreateMantlePoints();
        //Debug.Log("points generated");
    }
}
