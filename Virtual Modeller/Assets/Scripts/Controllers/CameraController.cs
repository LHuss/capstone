using UnityEngine;
using System;

public class CameraController : Singleton<CameraController> {

	readonly float rotationMinAngle = -30f;
	readonly float rotationMaxAngle = 90f;
	readonly float cameraMinDistance = 0.001f;
	readonly float cameraMaxDistance = 100f;

	readonly String mouseScrollWheelInput = "Mouse ScrollWheel";	
	readonly KeyCode disableCameraMovement = KeyCode.LeftShift;

	readonly float orbitSensitivity = 7.5f;
	readonly float zoomSensitivity = 5f;
	readonly float panSensitivity = 5f;
	readonly float mouseScrollDampening = 6f;	

	protected float cameraDistance = 1f;

	protected bool isMovementRestricted = false;

	protected Vector3 rotationVect;
	protected Quaternion startingAngle;
	protected Transform transformCamera;
	protected Transform transformParent;
	protected Vector3 startingPosition;

	void Awake(){
		Debug.Log("Initiating camera controls");
	}	
	
	public void ToggleMvmtRestriction(){
		isMovementRestricted=!isMovementRestricted;
	}

	public Transform TransformCamera {
		get {
			return this.transformCamera;
		}
		set {
			this.transformCamera = value;
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

	public float OrbitSensitivity {
		get {
			return this.orbitSensitivity;
		}
		set {
			this.orbitSensitivity = value;
		}
	}

	public float ZoomSensitivity {
		get {
			return this.zoomSensitivity;
		}
		set {
			this.zoomSensitivity = value;
		}
	}

	public float MouseScrollDampening {
		get {
			return this.mouseScrollDampening;
		}
		set {
			this.mouseScrollDampening = value;
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

	public float CameraDistance {
		get {
			return this.cameraDistance;
		}
		set {
			this.cameraDistance = value;
		}
	}

	public Vector3 ResetCamera(){
		this.transformCamera.position = this.startingPosition;
		this.cameraDistance = 1f;
		Debug.Log("new camera position: " + transformCamera.position);
		return this.transformCamera.position;
	}

	public Tuple<float, float> OrbitCamera(float xmovement, float ymovement){
		this.rotationVect.x += xmovement * this.orbitSensitivity;
		this.rotationVect.y -= ymovement * this.orbitSensitivity;

		if(this.rotationVect.y < rotationMinAngle){
			this.rotationVect.y = rotationMinAngle;
		}
		if(this.rotationVect.y > rotationMaxAngle){
			this.rotationVect.y = rotationMaxAngle;
		}
		return new Tuple<float, float>(this.rotationVect.x, this.rotationVect.y);
	}

	public float ZoomCamera(float msw){
		float scrollDepth = msw * this.zoomSensitivity;

		scrollDepth = scrollDepth * this.cameraDistance * 0.25f;

		this.cameraDistance += scrollDepth * -1f;

		if(this.cameraDistance < this.cameraMinDistance){
			this.cameraDistance = this.cameraMinDistance;
		}
		if(this.cameraDistance > this.cameraMaxDistance){
			this.cameraDistance = this.cameraMaxDistance;
		}
		return scrollDepth;
	}

	public void HandleCamera(){

		if(Input.GetKeyDown(disableCameraMovement)){
			ToggleMvmtRestriction();
		}

		if(!isMovementRestricted){

			float mouseScrollInputAmount = Input.GetAxis(mouseScrollWheelInput);	

			if(mouseScrollInputAmount!=0f || Input.GetKey("-") || Input.GetKey("=")){

				// Zoom camera using mouse wheel
				if(mouseScrollInputAmount!=0f){
					float zunits = ZoomCamera(mouseScrollInputAmount);
					//Debug.Log("Zooming by " + zunits + " units.");					
				}

				// Zoom camera using keyboard
				if(Input.GetKey("-")){ // zoom out with -				
					float zoomDist = Time.deltaTime*this.zoomSensitivity;
					float zunits = ZoomCamera(zoomDist);
					//Debug.Log(this.cameraDistance);
					//Debug.Log("Zooming by " + zunits + " units.");
					//Debug.Log(this.cameraDistance);					
				}

				if(Input.GetKey("=")){ // zoom in with +
					float zoomDist = Time.deltaTime*this.zoomSensitivity*-1;
					float zunits = ZoomCamera(zoomDist);
					//Debug.Log(this.cameraDistance);
					//Debug.Log("Zooming by " + zunits + " units.");
					//Debug.Log(this.cameraDistance);					
				}

				//zoom	
				this.transformCamera.localPosition = new Vector3(this.transformCamera.localPosition.x, this.transformCamera.localPosition.y, Mathf.Lerp(this.transformCamera.localPosition.z, this.cameraDistance*-1f, Time.deltaTime * this.mouseScrollDampening));
			}

			// Reset camera
			if(Input.GetKey("r")){
				Debug.Log("Resetting camera distance");
				ResetCamera();
			}

			// Prints the current camera position into the console
			if(Input.GetKey("p")){
				Debug.Log("Getting current camera position...");
				Debug.Log(this.transformCamera.position);
			}	
			
		}
	
	}

}
