using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
	private readonly string menuName = "ToolMenu";

	public GameObject toolSettingsUI;	
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.BackQuote)) {
			if (MenuController.Instance.GameIsPausedByMenu(menuName)) {
				DeactiveMenu(toolSettingsUI);
			} else {
				ActivateMenu(toolSettingsUI);
			}
		}
	}

	void ActivateMenu(GameObject menu) {
		menu.SetActive(true);
		MenuController.Instance.Pause(menuName);
	}

	void DeactiveMenu(GameObject menu) {
		menu.SetActive(false);
		MenuController.Instance.TryResume(menuName);
	}
}
