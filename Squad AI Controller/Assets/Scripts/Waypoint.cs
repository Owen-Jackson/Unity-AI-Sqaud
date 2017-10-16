using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waypoint : MonoBehaviour {
    public float score;
    public bool taken = false;
    public OffMeshLink myLink;

    public void SetTaken(bool setting)
    {
        taken = setting;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Squad_Member")
        {
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
}
