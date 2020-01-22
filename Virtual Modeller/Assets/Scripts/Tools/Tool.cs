using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using Leap.Unity.Query;
using UnityEngine;

public abstract class Tool : MonoBehaviour {
    protected GameObject tool;
    protected Rigidbody rb;
    protected ToolType type;
    protected Vector3 localScale;

    protected float toolSpeed = 10.0f;

    protected bool IsActive {
        get {
            return type == ToolController.Instance.ActiveToolType;
        }
    }

    public GameObject ToolObject {
        get {
            return this.tool;
        }
        set {
            this.tool = value;
        }
    }

    public ToolType Type {
        get {
            return type;
        }
        set {
            type = value;
        }
    }

	void Update () {
        if(IsActive) {
            UpdateTool();
            UpdateGameObject(); 
        } 
	}

    protected void SetupTool() {
		rb = tool.AddComponent<Rigidbody>();
		rb.freezeRotation = true;
		rb.detectCollisions = true;

		UpdateGameObject();
    }

    protected virtual void UpdateTool() {
        // Reset tool position to right in front of the camera when pressing space
        if (Input.GetKey(KeyCode.Space)) {
            ToolController.Instance.ForceUpdate();
        }

        // Only move with mouse if left control is held
        if (Input.GetKey(KeyCode.LeftControl)) {
            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");
            float dt = Time.deltaTime;
            float speed = toolSpeed * dt * ToolController.Instance.Size;

            float x = dx * speed;
            float y = dy * speed;
            Vector3 pos = ToolController.Instance.ToolPosition;
            if (Input.GetMouseButton(0)) {
                pos.z += y;
		    } else {
                pos.x += x;
                pos.y += y;
            }      
            ToolController.Instance.ToolPosition = pos;   
        }

        // Only move with Leap Motion Detector if at least one detector is active.
        InteractionManager im = InteractionManager.instance;
        var intController = im.interactionControllers
                .Query()
                .Where(controller => controller.isTracked).FirstOrDefault();
        if (!!intController) {
            Debug.Log(intController.position);
            ToolController.Instance.ToolPosition = intController.position;
        }
		
    }

    protected void UpdateGameObject() {
        tool.transform.position = ToolController.Instance.ToolPosition;
        tool.transform.localScale = localScale * ToolController.Instance.Size;
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Wat");
    }
}