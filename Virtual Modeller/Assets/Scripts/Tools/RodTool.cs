using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodTool : Tool {
	public void Awake () {
		Debug.Log("Creating Rod Tool");
		type = ToolType.TOOL_ROD;
		tool = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		CapsuleCollider tc = tool.GetComponent<CapsuleCollider>();
		tc.height *= 0.2f;
		tc.radius *= 0.15f;
		localScale = new Vector3(0.01f, 0.025f, 0.01f);
		SetupTool();
	}
}
