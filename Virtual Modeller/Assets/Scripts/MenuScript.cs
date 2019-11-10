using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
	public GameObject toolSettingsUI;	
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.BackQuote)) {
			if (MenuController.Instance.GameIsPaused) {
				DeactiveMenu(toolSettingsUI);
			} else {
				ActivateMenu(toolSettingsUI);
			}
		}
	}

	void ActivateMenu(GameObject menu) {
		menu.SetActive(true);
		MenuController.Instance.Pause();
		Debug.Log("Game Paused");
	}

	void DeactiveMenu(GameObject menu) {
		menu.SetActive(false);
		MenuController.Instance.Resume();
		Debug.Log("Game Resume");
	}
}
