using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waypoint : MonoBehaviour {
    public float allyScore = 0;
    public float enemyScore = 0;
    public float searchRadius = 50f;
    public bool taken = false;
    public GameObject owner = null;
    public OffMeshLink myLink;

    public void SetTaken(bool setting)
    {
        taken = setting;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == owner)
        {
            owner = null;
            taken = false;
        }
    }

    public void SetOffMeshLink(GameObject target)
    {
        myLink.startTransform = transform;
        myLink.endTransform = target.transform;
        myLink.biDirectional = true;
    }

    // Use this for initialization
    void Awake () {
        myLink = GetComponent<OffMeshLink>();
        myLink.autoUpdatePositions = true;
	}

    public float GetScore(string affinity)
    {
        if(affinity == "Enemy")
        {
            return enemyScore;
        }
        else if(affinity == "Ally")
        {
            return allyScore;
        }
        return 0f;
    }

    private float ThreatEvaluation(LayerMask layer)
    {
        float score = 0;
        Collider[] nearby = Physics.OverlapSphere(transform.position, searchRadius, 1<<layer);
        if (nearby.Length > 0)
        {
            for(int i = 0; i < nearby.Length;i++)
            {
                //if nearby actor is blocked, add to my score
                if (Physics.Linecast(nearby[i].transform.position, transform.position, LayerMask.NameToLayer("Cover")))
                {
                    score += 2;
                    //perform distance check to get how close each actor is to the waypoint
                    score += (Vector3.Distance(nearby[i].transform.position, transform.position));
                }

                //perform distance check to get how close each enemy is to the waypoint
                score += (Vector3.Distance(nearby[i].transform.position, transform.position)) * 0.5f;

            }
        }
        return score;
    }

    private float FriendlyCheck(LayerMask layer)
    {
        float score = 0;
        Collider[] nearby = Physics.OverlapSphere(transform.position, searchRadius, 1 << layer);
        if (nearby.Length > 0)
        {
            for (int i = 0; i < nearby.Length; i++)
            {
                //perform distance check to get how close each teammate is to the waypoint
                score += (searchRadius - Vector3.Distance(nearby[i].transform.position, transform.position)) *0.25f;

            }
        }
        return score;
    }

    private void Update()
    {
        allyScore = ThreatEvaluation(LayerMask.NameToLayer("Enemy"));
        allyScore += FriendlyCheck(LayerMask.NameToLayer("Player_Ally"));
        enemyScore = ThreatEvaluation(LayerMask.NameToLayer("Player_Ally"));
        enemyScore += FriendlyCheck(LayerMask.NameToLayer("Enemy"));
    }
}
