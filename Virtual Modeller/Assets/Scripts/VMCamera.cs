using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMCamera : MonoBehaviour {

	protected Transform transformCamera;
	protected Transform transformParent;
	protected Vector3 startingPosition;

	void Start(){
		Debug.Log("Assigning main camera");

		GameObject camera = GameObject.Find("Main Camera");
			
		this.transformCamera = camera.transform;
		this.transformParent = camera.transform.parent;
		this.startingPosition = new Vector3(transformCamera.position.x, transformCamera.position.y, transformCamera.position.z);
		Debug.Log("camera position: " + transformCamera.position);
		//Debug.Log("camera localposition: " + transformCamera.localPosition);
		CameraController.Instance.StartingPosition = this.startingPosition;
		CameraController.Instance.TransformCamera = this.transformCamera;
		CameraController.Instance.TransformParent = this.transformParent;
	}

	void Update(){
		CameraController.Instance.HandleCamera();		
	}

}
