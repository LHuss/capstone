using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuScript : MonoBehaviour {

	private Canvas CanvasObject;

	// Use this for initialization
	void Start () {
		CanvasObject = GetComponent<Canvas>();
		CanvasObject.enabled = !CanvasObject.enabled;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			CanvasObject.enabled = !CanvasObject.enabled;
		}
	}
}
