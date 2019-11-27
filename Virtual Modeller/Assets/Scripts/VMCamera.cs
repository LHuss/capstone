using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMCamera : MonoBehaviour {

	protected Transform transformCamera;
	protected Transform transformParent;

	void Start(){
		Debug.Log("Assigning main camera");
		this.transformCamera = this.transform;
		this.transformParent = this.transform.parent;		
	}

	void Update(){

	}

	void LateUpdate(){
		CameraController.Instance.HandleCamera(this.transformCamera, this.transformParent);
	}
}
