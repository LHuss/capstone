using UnityEngine;
using System;

public class MovementController : Singleton<MovementController> {

	readonly float DEFAULT_ROTATION_X = 0;
	readonly float DEFAULT_ROTATION_Y = 0;
	readonly float DEFAULT_ROTATION_Z = 0;
	readonly float objectMinDistance = 0.01f;
	readonly float objectMaxDistance = 100f;

	readonly String mouseScrollWheelInput = "Mouse ScrollWheel";	
	readonly String mouseXAxisInput = "Mouse X";
	readonly String mouseYAxisInput = "Mouse Y";
	readonly KeyCode disableObjectMovement = KeyCode.LeftShift;

	readonly float zoomSensitivity = 2f;
	readonly float mouseScrollDampening = 6f;
	readonly float panSensitivity = 2f;
	readonly float rotationSpeed = 8f;
	
	protected bool isMovementRestricted = true;	
	protected float objectDistance = 0.2f;
	protected Vector3 rotationVect;
	protected Quaternion startingAngle;
	protected Transform transformObject;
	protected Transform transformParent;
	protected Vector3 startingPosition;

	void Awake(){
		Debug.Log("Initiating controls");		
		Debug.Log("Starting rotation position: " + this.rotationVect.x + ", " + this.rotationVect.y);
	}	
	
	public void RestrictMovement(){
		isMovementRestricted = false;
	}

	public void UnrestrictMovement(){
		isMovementRestricted = true;
	}

	public Transform TransformObject {
		get {
			return this.transformObject;
		}
		set {
			this.transformObject = value;
		}
	}

	public Transform TransformParent {
		get {
			return this.transformParent;
		}
		set {
			this.transformParent = value;
		}
	}

	public Quaternion StartingAngle {
		get {
			return this.startingAngle;
		}
		set {
			this.startingAngle = value;
		}
	}

	public Vector3 StartingPosition {
		get {
			return this.startingPosition;
		}
		set {
			this.startingPosition = value;
		}
	}

	public float PanSensitivity {
		get {
			return this.panSensitivity;
		}
	}

	public float RotationSpeed {
		get {
			return this.rotationSpeed;
		}
	}

	public Vector3 RotationVect {
		get {
			return this.rotationVect;
		}
		set {
			this.rotationVect = value;
		}
	}

	public bool IsMovementRestricted {
		get {
			return this.isMovementRestricted;
		}
		set {
			this.isMovementRestricted = !this.isMovementRestricted;
		}
	}

	public float ZoomSensitivity {
		get {
			return this.zoomSensitivity;
		}
	}

	public float MouseScrollDampening {
		get {
			return this.mouseScrollDampening;
		}
	}


	public float ObjectDistance {
		get {
			return this.objectDistance;
		}
		set {
			this.objectDistance = value;
		}
	}


	public Tuple<float, float> ResetPosition(){
		this.transformObject.rotation = Quaternion.Lerp(this.transformObject.rotation, this.startingAngle, Time.time*this.rotationSpeed);
		this.transformObject.position = startingPosition;
		this.rotationVect.x = DEFAULT_ROTATION_X;
		this.rotationVect.y = DEFAULT_ROTATION_Y;
		this.rotationVect.z = DEFAULT_ROTATION_Z;
		this.objectDistance = 0.2f;
		Debug.Log(transformObject.position);
		return new Tuple<float, float>(this.transformObject.rotation.x, this.transformObject.rotation.y);
	}

	public Tuple<float, float, float> RotateObject(float xmovement, float ymovement, float zmovement){

		this.rotationVect.x += xmovement * this.rotationSpeed;
		this.rotationVect.y -= ymovement * this.rotationSpeed;
		this.rotationVect.z += zmovement * this.rotationSpeed;

		return new Tuple<float, float, float>(this.rotationVect.x, this.rotationVect.y, this.rotationVect.z);
	}

	public float ZoomObject(float msw){
		float scrollDepth = msw * this.zoomSensitivity;

		scrollDepth = scrollDepth * this.objectDistance * 0.25f;

		this.objectDistance += scrollDepth * 1f;
		Debug.Log(this.objectDistance);
		if(this.objectDistance < this.objectMinDistance){
			this.objectDistance = this.objectMinDistance;
		}
		if(this.objectDistance > this.objectMaxDistance){
			this.objectDistance = this.objectMaxDistance;
		}

		return scrollDepth;
	}

	public void HandleObject(){

		if(Input.GetKeyDown(disableObjectMovement)){
			if(isMovementRestricted)
				UnrestrictMovement();
			else
				RestrictMovement();
		}

		if(!isMovementRestricted){

			float mouseXInputAmount = Input.GetAxis(mouseXAxisInput);
			float mouseYInputAmount = Input.GetAxis(mouseYAxisInput);
			float mouseScrollInputAmount = Input.GetAxis(mouseScrollWheelInput);	

			if(mouseScrollInputAmount!=0f || Input.GetKey("-") || Input.GetKey("=")){

				// Zoom using mouse wheel
				if(mouseScrollInputAmount!=0f){
					float zunits = ZoomObject(mouseScrollInputAmount);
					//Debug.Log("Zooming by " + zunits + " units.");					
				}

				// Zoom using keyboard
				if(Input.GetKey("-")){ // zoom in with -			
					float zoomDist = Time.deltaTime*this.zoomSensitivity*-1;
					float zunits = ZoomObject(zoomDist);
					//Debug.Log(this.objectDistance);
					//Debug.Log("Zooming by " + zunits + " units.");
					//Debug.Log(this.objectDistance);					
				}

				if(Input.GetKey("=")){ // zoom out with =
					float zoomDist = Time.deltaTime*this.zoomSensitivity;
					float zunits = ZoomObject(zoomDist);
					//Debug.Log(this.objectDistance);
					//Debug.Log("Zooming by " + zunits + " units.");
					//Debug.Log(this.objectDistance);					
				}

				//zoom	
				this.transformObject.localPosition = new Vector3(this.transformObject.localPosition.x, this.transformObject.localPosition.y, Mathf.Lerp(this.transformObject.localPosition.z, this.objectDistance*1f, Time.deltaTime * this.mouseScrollDampening));
			}

			
			// Rotate using mouse
			if(mouseXInputAmount!=0 || mouseYInputAmount!=0){
				RotateObject(mouseYInputAmount, mouseXInputAmount, 0);
			}

			float xRot= Input.GetAxis("Vertical"); // Rotate along the Y axis
			float yRot= Input.GetAxis("Horizontal"); // Rotate along the X axis

			// Rotate using keyboard
			if(Input.GetKey("left")){ 
				RotateObject(Time.deltaTime*xRot, yRot, 0);
			}
			if(Input.GetKey("right")){ 
				RotateObject(Time.deltaTime*-xRot, yRot, 0);
			}
			if(Input.GetKey("up")){ 
				RotateObject(xRot, Time.deltaTime*yRot, 0);
			}
			if(Input.GetKey("down")){ 
				RotateObject(xRot, Time.deltaTime*-yRot, 0);
			}

			// Pan left or right using keyboard
			if(Input.GetKey("a")){
				Debug.Log("Panning left..");
				this.transformObject.Translate(Vector3.left * this.panSensitivity * Time.deltaTime, Space.World);
				Debug.Log(transformObject.position);
			}
			if(Input.GetKey("d")){
				Debug.Log("Panning right..");
				this.transformObject.Translate(Vector3.right * this.panSensitivity * Time.deltaTime, Space.World);
				Debug.Log(transformObject.position);
			}

			// Reset camera
			if(Input.GetKey("r")){
				Debug.Log("Resetting camera angle");
				Debug.Log("Starting position: "+ this.startingPosition);
				Debug.Log("Reverting to position: "+ this.transformObject.position);
				ResetPosition();
			}

			// Prints the current camera position into the console
			if(Input.GetKey("p")){
				Debug.Log("Getting current object position...");
				Debug.Log(this.transformObject.position);
			}

			Quaternion q = Quaternion.Euler(rotationVect.x, rotationVect.y, rotationVect.z);
			this.transformObject.rotation = Quaternion.Lerp(this.transformObject.rotation, q, Time.deltaTime*this.rotationSpeed);
		}
	
	}
}
