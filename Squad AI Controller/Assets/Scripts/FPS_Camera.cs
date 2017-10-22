using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Camera : MonoBehaviour {
    Vector2 mouseLook;
    Vector2 smoothVec;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;
    GameObject character;
    
	// Use this for initialization
	void Start () {
        character = this.transform.parent.gameObject;
	}
	
    void CamRotation()
    {
        Vector2 rotDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        rotDelta = Vector2.Scale(rotDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothVec.x = Mathf.Lerp(smoothVec.x, rotDelta.x, 1.0f / smoothing);
        smoothVec.y = Mathf.Lerp(smoothVec.y, rotDelta.y, 1.0f / smoothing);
        mouseLook += smoothVec;
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90.0f, 90.0f);  //stops camera rotating too far

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }

	// Update is called once per frame
	void Update () {
        CamRotation();
	}
}