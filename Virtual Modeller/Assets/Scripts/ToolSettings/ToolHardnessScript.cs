using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolHardnessScript : DisplayValueScript {
	public void UpdateHardness (float value) {
		ToolController.Instance.Hardness = value;
		this.UpdateText(value);
	}	
}
