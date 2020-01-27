using UnityEngine;
using System;

public class MovementController : Singleton<MovementController> {

	readonly float DEFAULT_ROTATION_X = 0;
	readonly float DEFAULT_ROTATION_Y = 0;
	readonly float DEFAULT_ROTATION_Z = 0;

	readonly String mouseXAxisInput = "Mouse X";
	readonly String mouseYAxisInput = "Mouse Y";
	readonly KeyCode disableObjectMovement = KeyCode.LeftShift;

	readonly float panSensitivity = 5f;
	readonly float rotationSpeed = 8f;
	
	protected bool isMovementRestricted = false;	

	protected Vector3 rotationVect;
	protected Quaternion startingAngle;
	protected Transform transformObject;
	protected Transform transformParent;
	protected Vector3 startingPosition;

	void Awake(){
		Debug.Log("Initiating controls");		
		Debug.Log("Starting rotation position: " + this.rotationVect.x + ", " + this.rotationVect.y);
	}	
	
	public void ToggleMvmtRestriction(){
		isMovementRestricted=!isMovementRestricted;
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

	public float RotationSpeed {
		get {
			return this.rotationSpeed;
		}
		set {
			this.rotationSpeed = value;
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

	public Tuple<float, float> ResetPosition(){
		this.transformObject.rotation = Quaternion.Lerp(this.transformObject.rotation, this.startingAngle, Time.time*this.rotationSpeed);
		this.transformObject.position = startingPosition;
		this.rotationVect.x = DEFAULT_ROTATION_X;
		this.rotationVect.y = DEFAULT_ROTATION_Y;
		this.rotationVect.z = DEFAULT_ROTATION_Z;
		Debug.Log(transformObject.position);
		return new Tuple<float, float>(this.transformObject.rotation.x, this.transformObject.rotation.y);
	}

	public Tuple<float, float> RotateObject(float xmovement, float ymovement){

		this.rotationVect.x += xmovement * this.rotationSpeed;
		this.rotationVect.y -= ymovement * this.rotationSpeed;	

		return new Tuple<float, float>(this.rotationVect.x, this.rotationVect.y);
	}

	public void HandleObject(){

		if(Input.GetKeyDown(disableObjectMovement)){
			ToggleMvmtRestriction();
		}

		if(!isMovementRestricted){

			float mouseXInputAmount = Input.GetAxis(mouseXAxisInput);
			float mouseYInputAmount = Input.GetAxis(mouseYAxisInput);
			
			// Rotate using mouse
			if(mouseXInputAmount!=0 || mouseYInputAmount!=0){
				RotateObject(mouseYInputAmount, mouseXInputAmount);
			}

			float xRot= Input.GetAxis("Vertical"); // Rotate along the Y axis
			float yRot= Input.GetAxis("Horizontal"); // Rotate along the X axis

			// Rotate using keyboard
			if(Input.GetKey("left")){ 
				RotateObject(Time.deltaTime*xRot, yRot);
			}
			if(Input.GetKey("right")){ 
				RotateObject(Time.deltaTime*-xRot, yRot);
			}
			if(Input.GetKey("up")){ 
				RotateObject(xRot, Time.deltaTime*yRot);
			}
			if(Input.GetKey("down")){ 
				RotateObject(xRot, Time.deltaTime*-yRot);
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

			Quaternion q = Quaternion.Euler(rotationVect.x, rotationVect.y, 0);
			this.transformObject.rotation = Quaternion.Lerp(this.transformObject.rotation, q, Time.deltaTime*this.rotationSpeed);

		}
	
	}
}
