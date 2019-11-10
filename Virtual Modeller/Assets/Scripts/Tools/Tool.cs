using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour {
    protected GameObject tool;
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

    protected virtual void UpdateTool() {
        // Only move if left shift is held
        if (Input.GetKey(KeyCode.LeftShift)) {
            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");
            float dt = Time.deltaTime;
            float speed = toolSpeed * dt;

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
    }

    protected void UpdateGameObject() {
        tool.transform.position = ToolController.Instance.ToolPosition;
        tool.transform.localScale = localScale * ToolController.Instance.Size;
    }
}