using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGravity : MonoBehaviour {

	// Use this for initialization
	public void init () {
		GameObject hugeSphere = GameObject.Find("HugeSphere");
		if (hugeSphere.GetComponent<Rigidbody>()==null){
			hugeSphere.AddComponent<Rigidbody>();
		}
		Rigidbody rigidbody = hugeSphere.GetComponent<Rigidbody>();
		rigidbody.useGravity = true;
	}
	
	/*
	// Update is called once per frame
	void Update () {
		
	}*/
}
