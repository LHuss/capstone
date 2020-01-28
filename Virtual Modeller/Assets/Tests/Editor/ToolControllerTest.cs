using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;

public class ToolControllerTest {
	void InitializeController() {
		ToolController.Instance.Size = 1f;
		Dictionary<ToolType, Tool> tools = new Dictionary<ToolType, Tool>();		

		GameObject go = new GameObject();
		Tool cube = go.AddComponent<CubeTool>();
		((CubeTool) cube).Awake();
		tools[cube.Type] = cube;
		Tool rod = go.AddComponent<RodTool>();
		((RodTool) rod).Awake();
		tools[rod.Type] = rod;
		Tool sphere = go.AddComponent<SphereTool>();
		((SphereTool) sphere).Awake();
		tools[sphere.Type] = sphere;

		ToolController.Instance.Tools = tools;
		ToolController.Instance.ActiveToolType = ToolType.TOOL_CUBE;
	}

	[Test]
	public void ForceUpdate() {
		InitializeController();
		ToolController tc = ToolController.Instance;

		tc.ForceUpdate();

		Vector3 expectedPosition = Camera.main.gameObject.transform.position + Camera.main.gameObject.transform.forward * 5;
		Assert.AreEqual(expectedPosition, tc.ToolPosition);

		Assert.AreEqual(ToolType.TOOL_CUBE, tc.ActiveToolType);

		foreach(KeyValuePair<ToolType, Tool> tool in tc.Tools) {
			bool expectedActive = ((Tool) tool.Value).Type == ToolType.TOOL_CUBE;
			Assert.AreEqual(expectedActive, ((Tool) tool.Value).IsActive);
		}
	}
}
