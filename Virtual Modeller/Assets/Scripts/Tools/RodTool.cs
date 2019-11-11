using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodTool : Tool {
	public void Awake () {
		Debug.Log("Creating Rod Tool");
		type = ToolType.TOOL_ROD;
		tool = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		localScale = new Vector3(0.2f, 0.5f, 0.2f);
		UpdateGameObject();
	}
}
