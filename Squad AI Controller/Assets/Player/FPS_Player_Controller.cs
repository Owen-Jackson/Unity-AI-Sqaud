using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Player_Controller : MonoBehaviour {
    public float moveSpeed = 5.0f;
    public Squad_Controller mySquad;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}
	
    void Movement()
    {
        float forwardBack = Input.GetAxis("Vertical") * moveSpeed;
        float strafe = Input.GetAxis("Horizontal") * moveSpeed;
        forwardBack *= Time.deltaTime;
        strafe *= Time.deltaTime;

        transform.Translate(strafe, 0, forwardBack);
    }

    //Basic "Go here" command for squad
    void MoveHereCommand()
    {
        //Raycast from camera pos and rot
        Transform aimFrom = transform.Find("Player_Camera");
        Ray ray = new Ray(aimFrom.position, aimFrom.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {          
            if (hit.collider != null)
            {
                if (hit.transform.tag == "Cover")
                {
                    mySquad.MoveSquad(hit.point, hit.transform.gameObject);
                    Debug.Log("send to cover");
                }
                else
                {
                    mySquad.MoveSquad(hit.point);
                    Debug.Log("send to point");
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
        Movement();
        if(Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if(Input.GetKeyDown("r"))
        {
            MoveHereCommand();
        }
	}
}