using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTool : Tool {
	public void Awake () {
		Debug.Log("Creating Sphere Tool");
		type = ToolType.TOOL_SPHERE;
		tool = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		tool.GetComponent<SphereCollider>().radius *= 0.3f;
		localScale = new Vector3(0.05f, 0.05f, 0.05f);
		SetupTool();
	}
}
