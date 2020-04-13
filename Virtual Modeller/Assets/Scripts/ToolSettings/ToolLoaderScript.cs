using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolLoaderScript : MonoBehaviour {
	void Start () {
		Debug.Log("Creating tools");
		Tool cube = gameObject.AddComponent<CubeTool>();
		ToolController.Instance.Tools[cube.Type] = cube;
		Tool rod = gameObject.AddComponent<RodTool>();
		ToolController.Instance.Tools[rod.Type] = rod;
		Tool sphere = gameObject.AddComponent<SphereTool>();
		ToolController.Instance.Tools[sphere.Type] = sphere;
		ToolController.Instance.ActiveToolType = ToolType.TOOL_HAND;
		ToolController.Instance.ForceUpdate();
	}
}
