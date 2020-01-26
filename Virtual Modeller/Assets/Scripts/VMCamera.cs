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

		GameObject camera = GameObject.Find("Main Camera");
			
		this.transformCamera = camera.transform;
		this.transformParent = camera.transform.parent;
		//this.startingPosition = new Vector3(transformCamera.position.x, transformCamera.position.y, transformCamera.position.z);
		this.startingAngle = transformParent.rotation;
		Debug.Log(transformCamera.position);
		Debug.Log(transformCamera.localPosition);
		CameraController.Instance.StartingAngle = this.startingAngle;
		CameraController.Instance.TransformCamera = this.transformCamera;
		CameraController.Instance.TransformParent = this.transformParent;
	}

	void LateUpdate(){
		CameraController.Instance.HandleCamera();		
	}

}
