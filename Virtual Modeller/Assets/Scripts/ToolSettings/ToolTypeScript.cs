using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTypeScript : MonoBehaviour {
	public void UpdateActiveToolType(int type) {
		ToolController.Instance.ActiveToolType = (ToolType) type;
	}
}
