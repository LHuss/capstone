using UnityEngine;
using System;

public class CameraController : Singleton<CameraController> {

	protected Vector3 rotationVect;

	readonly float DEFAULT_CAMERA_DISTANCE = 1f;
	readonly Vector3 DEFAULT_POSITION = new Vector3(0, 1.4f , -0.2f);
	readonly float DEFAULT_ROTATION_X = 0;
	readonly float DEFAULT_ROTATION_Y = 0;

	protected float cameraDistance = 1f;

	protected float orbitSensitivity = 7.5f;
	protected float zoomSensitivity = 5f;
	protected float panSensitivity = 5f;
	protected float mouseScrollDampening = 6f;
	protected float rotationSpeed = 8f;
	
	protected bool isMovementRestricted = false;

	protected float rotationMinAngle = -30f;
	protected float rotationMaxAngle = 90f;
	protected float cameraMinDistance = 0.1f;
	protected float cameraMaxDistance = 100f;

	protected String mouseXAxisInput = "Mouse X";
	protected String mouseYAxisInput = "Mouse Y";
	protected String mouseScrollWheelInput = "Mouse ScrollWheel";
	
	protected KeyCode disableCameraMovement = KeyCode.LeftShift;

	protected Quaternion startingAngle;
	protected Transform transformCamera;
	protected Transform transformParent;

	void Awake(){
		Debug.Log("Initiating camera controls");		
		Debug.Log("Starting rotation position: " + this.rotationVect.x + ", " + this.rotationVect.y);
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

	public float CameraDistance {
		get {
			return this.cameraDistance;
		}
		set {
			this.cameraDistance = value;
		}
	}

	public Tuple<float, float, float> ResetCamera(){
		this.transformParent.rotation = Quaternion.Lerp(this.transformParent.rotation, this.startingAngle, Time.time*this.rotationSpeed);
		this.transformParent.position = DEFAULT_POSITION;
		this.rotationVect.x = DEFAULT_ROTATION_X;
		this.rotationVect.y = DEFAULT_ROTATION_Y;
		this.cameraDistance = DEFAULT_CAMERA_DISTANCE;
		Debug.Log(transformCamera.position);
		return new Tuple<float, float, float>(this.rotationVect.x, this.rotationVect.y, this.cameraDistance);
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
			float mouseXInputAmount = Input.GetAxis(mouseXAxisInput);
			float mouseYInputAmount = Input.GetAxis(mouseYAxisInput);
			
			// Orbit using mouse
			if(mouseXInputAmount!=0 || mouseYInputAmount!=0){
				OrbitCamera(mouseXInputAmount, mouseYInputAmount);
			}

			// Orbit using keyboard
			if(Input.GetKey("left")){ 
				OrbitCamera(Time.deltaTime*this.orbitSensitivity*5f, 0f);
			}
			if(Input.GetKey("right")){ 
				OrbitCamera(Time.deltaTime*this.orbitSensitivity*-1*5f, 0f);
			}
			if(Input.GetKey("up")){ 
				OrbitCamera(0f, Time.deltaTime*this.orbitSensitivity*-1*5f);
			}
			if(Input.GetKey("down")){ 
				OrbitCamera(0f, Time.deltaTime*this.orbitSensitivity*5f);
			}

			// Zoom camera using mouse wheel
			if(mouseScrollInputAmount!=0f){
				Debug.Log("Zooming by " + ZoomCamera(mouseScrollInputAmount) + " units.");		
			}

			// Pan left or right using keyboard
			if(Input.GetKey("a")){
				Debug.Log("Panning left..");
				this.transformParent.Translate(Vector3.left * this.panSensitivity * Time.deltaTime, Space.Self);
				Debug.Log(transformCamera.position);
			}
			if(Input.GetKey("d")){
				Debug.Log("Panning right..");
				this.transformParent.Translate(Vector3.right * this.panSensitivity * Time.deltaTime, Space.Self);
				Debug.Log(transformCamera.position);
			}

			// Zoom camera using keyboard
			if(Input.GetKey("-")){ // zoom out with -				
				float zoomDist = Time.deltaTime*this.zoomSensitivity;
				Debug.Log(this.cameraDistance);
				Debug.Log("Zooming by " + ZoomCamera(zoomDist) + " units.");
				Debug.Log(this.cameraDistance);
			}
			if(Input.GetKey("=")){ // zoom in with +
				float zoomDist = Time.deltaTime*this.zoomSensitivity*-1;
				Debug.Log(this.cameraDistance);
				Debug.Log("Zooming by " + ZoomCamera(zoomDist) + " units.");
				Debug.Log(this.cameraDistance);
			}

			// Reset camera
			if(Input.GetKey("r")){
				Debug.Log("Resetting camera angle");
				ResetCamera();
			}

			// Prints the current camera position into the console
			if(Input.GetKey("p")){
				Debug.Log("Getting current camera position...");
				Debug.Log(this.transformCamera.position);
			}

			// rotation
			Quaternion q = Quaternion.Euler(rotationVect.y, rotationVect.x, 0);
			this.transformParent.rotation = Quaternion.Lerp(this.transformParent.rotation, q, Time.deltaTime*this.rotationSpeed);

			//zoom		
			this.transformCamera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this.transformCamera.localPosition.z, this.cameraDistance*-1f, Time.deltaTime * this.mouseScrollDampening));		
			
		}
	
	}

}
