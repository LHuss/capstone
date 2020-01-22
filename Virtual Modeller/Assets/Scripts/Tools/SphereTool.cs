using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTool : Tool {
	public void Awake () {
		Debug.Log("Creating Sphere Tool");
		type = ToolType.TOOL_SPHERE;
		tool = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		localScale = new Vector3(0.05f, 0.05f, 0.05f);
		SetupTool();
	}
}
