﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {
    public float rotateSpeed = 0.2f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0,rotateSpeed,0));
	}
}
