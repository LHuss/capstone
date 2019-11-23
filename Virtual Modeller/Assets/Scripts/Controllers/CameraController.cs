using UnityEngine;
using System;

public class CameraController : Singleton<CameraController> {

	protected Vector3 rotationVect;

	protected float cameraDistance = 10f;

	protected float orbitSensitivity = 4f;
	protected float zoomSensitivity = 2f;
	protected float panSensitivity = 5f;
	protected float mouseScrollDampening = 6f;
	protected float rotationSpeed = 8f;
	
	protected bool isMovementRestricted = false;

	protected float rotationMinAngle = 0f;
	protected float rotationMaxAngle = 90f;
	protected float cameraMinDistance = 2f;
	protected float cameraMaxDistance = 100f;

	protected String mouseXAxisInput = "Mouse X";
	protected String mouseYAxisInput = "Mouse Y";
	protected String mouseScrollWheelInput = "Mouse ScrollWheel";
	
	protected KeyCode disableCamera = KeyCode.LeftShift;

	void Awake(){
		Debug.Log("Initiating camera controls");
	}	
	
	public void ToggleMvmtRestriction(){
		isMovementRestricted=!isMovementRestricted;
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

	public void OrbitCamera(float xmovement, float ymovement){
		rotationVect.x += xmovement * orbitSensitivity;
		rotationVect.y -= ymovement * orbitSensitivity;

		if(rotationVect.y < rotationMinAngle){
			rotationVect.y = rotationMinAngle;
		}
		if(rotationVect.y > rotationMaxAngle){
			rotationVect.y = rotationMaxAngle;
		}
	}

	public float ZoomCamera(float msw){
		float scrollDepth = msw * zoomSensitivity;

		scrollDepth = scrollDepth * this.cameraDistance * 0.25f;

		this.cameraDistance += scrollDepth * -1f;

		if(cameraDistance < cameraMinDistance){
			cameraDistance = cameraMinDistance;
		}
		if(cameraDistance > cameraMaxDistance){
			cameraDistance = cameraMaxDistance;
		}
		return scrollDepth;
	}

	public void HandleCamera(Transform tc, Transform tp){

		Vector3 pos = tc.position;

		if(Input.GetKeyDown(disableCamera)){
			ToggleMvmtRestriction();
		}

		if(!isMovementRestricted){

			float mouseXInputAmount = Input.GetAxis(mouseXAxisInput);
			float mouseYInputAmount = Input.GetAxis(mouseYAxisInput);
			float mouseScrollInputAmount = Input.GetAxis(mouseScrollWheelInput);
			float kbPan = Input.GetAxis("Horizontal") * panSensitivity * Time.deltaTime * 20f;

			// Orbit camera using mouse
			if(mouseXInputAmount!=0 || mouseYInputAmount!=0){
				OrbitCamera(mouseXInputAmount, mouseYInputAmount);				
			}

			// Zoom camera using mouse wheel
			if(mouseScrollInputAmount!=0f){
				Debug.Log("Zooming by " + ZoomCamera(mouseScrollInputAmount) + " units.");		
			}

			// Pan left and right using keyboard
			if(Input.GetKey("a")){
				Debug.Log("Panning left..");
				Vector3 strafeLeft = new Vector3(kbPan, 0f, 0f);
				tp.position = strafeLeft;					

			}

			// Orbit camera using keyboard
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

			// Zoom camera using keyboard
			if(Input.GetKey("-")){ // zoom out with -
				Debug.Log("Zooming by " + ZoomCamera(Time.deltaTime*this.zoomSensitivity) + " units.");
			}
			if(Input.GetKey("=")){ // zoom in with +
				Debug.Log("Zooming by " + ZoomCamera(Time.deltaTime*this.zoomSensitivity*-1) + " units.");
			}
			
		}

		Quaternion q = Quaternion.Euler(rotationVect.y, rotationVect.x, 0);

		//rotation
		tp.rotation = Quaternion.Lerp(tp.rotation, q, Time.deltaTime*rotationSpeed);

		//zoom		
		tc.localPosition = new Vector3(0f, 0f, Mathf.Lerp(tc.localPosition.z, this.cameraDistance*-1f, Time.deltaTime * this.mouseScrollDampening));
	
	}

}
