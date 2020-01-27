using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour {

	protected Transform transformObject;
	protected Transform transformParent;
	protected Vector3 startingPosition;
	protected Quaternion startingAngle;

	void Start() {
		Debug.Log("Assigning object");
			
		this.transformObject = this.transform;
		this.transformParent = this.transform.parent;
		this.startingPosition = new Vector3(transformObject.position.x, transformObject.position.y, transformObject.position.z);
		this.startingAngle = transformParent.rotation;
		Debug.Log("Starting position: "+ this.startingPosition);
		//Debug.Log(transformObject.localPosition);
		MovementController.Instance.StartingAngle = this.startingAngle;
		MovementController.Instance.StartingPosition = this.startingPosition;
		MovementController.Instance.TransformObject = this.transformObject;
		MovementController.Instance.TransformParent = this.transformParent;
	}
	
	void Update() {
		MovementController.Instance.HandleObject();
	}
}
