using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSizeScript : DisplayValueScript {
	public void UpdateSize (float value) {
		ToolController.Instance.Size = value;
		this.UpdateText(value);
	}	
}
