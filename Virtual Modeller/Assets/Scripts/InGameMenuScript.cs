using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuScript : MonoBehaviour {
	private readonly string menuName = "ImportMenu";

	private Canvas CanvasObject;

	// Use this for initialization
	void Start () {
		CanvasObject = GetComponent<Canvas>();
		// Enable by default
		ActivateCanvas();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (MenuController.Instance.GameIsPausedByMenu(menuName)) {
				DeactiveCanvas();
			} else {
				ActivateCanvas();
			}
		}
	}

	void ActivateCanvas() {
		CanvasObject.enabled = true;
		MenuController.Instance.Pause(menuName);
	}

	void DeactiveCanvas() {
		CanvasObject.enabled = false;
		MenuController.Instance.TryResume(menuName);
	}
}
