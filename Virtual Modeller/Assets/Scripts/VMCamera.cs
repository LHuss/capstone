using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMCamera : MonoBehaviour {

	protected Transform transformCamera;
	protected Transform transformParent;
	protected Vector3 startingPosition;
	protected Quaternion startingAngle;

	void Start(){
		Debug.Log("Assigning main camera");
			
		this.transformCamera = this.transform;
		this.transformParent = this.transform.parent;
		//this.startingPosition = new Vector3(transformCamera.position.x, transformCamera.position.y, transformCamera.position.z);
		this.startingAngle = transformParent.rotation;
		Debug.Log(transformCamera.position);
		Debug.Log(transformCamera.localPosition);
	}

	void LateUpdate(){
		CameraController.Instance.HandleCamera(this.transformCamera, this.transformParent, this.startingAngle);		
	}
	
}
