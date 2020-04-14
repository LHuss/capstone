using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using Leap.Unity.Query;
using Leap.Unity;

public class ToolController : Singleton<ToolController> {
	private float hardness;
	private float size;
	private Vector3 toolPosition;
	private Dictionary<ToolType, Tool> tools;
	private ToolType activeToolType;

	public ToolType ActiveToolType {
		get {
			return this.activeToolType;
		}
		set {
			if (this.activeToolType != value) {
				this.activeToolType = value;
				UpdateActive();
			}
		}
	}

	public Dictionary<ToolType, Tool> Tools {
		get {
			return this.tools;
		}
		set {
			this.tools = value;
		}
	}

	public float Hardness {
		get {
			return this.hardness;
		}
		set {
			this.hardness = value;
		}
	}

	public float Size {
		get {
			return this.size;
		}
		set {
			this.size = value;
		}
	}

	public Vector3 ToolPosition {
		get {
			return toolPosition;
		}
		set {
			toolPosition = value;
		}
	}

	public void ForceUpdate() {
		UpdateToolPosition();
		UpdateActive();
	}

	private void Awake () {
		Debug.Log("Setting initial tool position to that of the camera + cameraLookAt");
		UpdateToolPosition();
		tools = new Dictionary<ToolType, Tool>();
		size = 1f;
	}
	
	private void UpdateToolPosition() {
		if (activeToolType != ToolType.TOOL_HAND) {
			Vector3 cameraLookAt = Camera.main.gameObject.transform.forward;
			float maxLookAtValue = Mathf.Max(cameraLookAt.x, cameraLookAt.y, cameraLookAt.z);

			// Normalize the vector by it's maximum value;
			Vector3 boundedLookAt = cameraLookAt / maxLookAtValue; 

			// Shift tool to right in front of the camera's lookAt, and a bit up and to the side 
			Vector3 desiredLookAt = boundedLookAt * 0.1f + new Vector3(0.1f, 0.05f, 0f); 
			toolPosition = Camera.main.gameObject.transform.position + desiredLookAt;
		}
	}

	private void UpdateActive() {
		Debug.Log("Switching tools to " + activeToolType);
		foreach(KeyValuePair<ToolType, Tool> tool in tools) {
			bool active = tool.Key == activeToolType;
			tools[tool.Key].ToolObject.SetActive(active);
		}

    
		HandModelManager[] handModelManagers  = FindObjectsOfType<HandModelManager>();
		bool shouldBeEnabled = activeToolType == ToolType.TOOL_HAND;
		foreach (HandModelManager manager in handModelManagers) {
			manager.GraphicsEnabled = shouldBeEnabled;
		}
	}
}
