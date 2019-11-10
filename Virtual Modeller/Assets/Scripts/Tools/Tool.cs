using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour {
    protected GameObject tool;
    protected ToolType type;
    protected Vector3 localScale;

    public float toolSpeed = 10.0f;

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
        // Only do calculations if inputs are detected
        if (!Input.anyKey || Input.GetMouseButton(0)) {
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

            UpdateGameObject();  
        }
	} 

    protected void UpdateGameObject() {
        tool.transform.position = ToolController.Instance.ToolPosition;
        tool.transform.localScale = localScale * ToolController.Instance.Size;
    }
}