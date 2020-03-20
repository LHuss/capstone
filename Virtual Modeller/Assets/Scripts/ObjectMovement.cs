using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour {
	readonly KeyCode disableObjectMovement = KeyCode.LeftShift;

	protected Transform transformObject;
	protected Transform transformParent;
	protected Vector3 startingPosition;
	protected Quaternion startingAngle;

	void Start() {
		Debug.Log("Assigning object");
			
		this.transformObject = this.transform;
		this.transformParent = this.transform.parent;
		this.startingPosition = new Vector3(transformObject.position.x, transformObject.position.y, transformObject.position.z);
		this.startingAngle = transformObject.rotation;
		Debug.Log("Starting position: "+ this.startingPosition);
		//Debug.Log(transformObject.localPosition);
		MovementController.Instance.StartingAngle = this.startingAngle;
		MovementController.Instance.StartingPosition = this.startingPosition;
		MovementController.Instance.TransformObject = this.transformObject;
		MovementController.Instance.TransformParent = this.transformParent;
	}
	
	void Update() {
		// Handle object movement using gestures if mouse movement is restricted
		if(Input.GetKeyDown(disableObjectMovement)){
			Debug.Log(MovementController.Instance.IsMovementRestricted);
			if(MovementController.Instance.IsMovementRestricted)
				MovementController.Instance.UnrestrictMovement();
			else
				MovementController.Instance.RestrictMovement();
		}

		if(MovementController.Instance.IsMovementRestricted)
			GestureController.Instance.HandleGestures();
		else
			MovementController.Instance.HandleObject();
	}
}
