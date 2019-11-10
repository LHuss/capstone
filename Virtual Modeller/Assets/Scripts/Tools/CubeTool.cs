using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTool : Tool {
	public void Awake () {
		Debug.Log("Creating Cube Tool");
		type = ToolType.TOOL_CUBE;
		tool = GameObject.CreatePrimitive(PrimitiveType.Cube);
		localScale = new Vector3(1.0f, 1.0f, 1.0f);
		UpdateGameObject();
	}
}
