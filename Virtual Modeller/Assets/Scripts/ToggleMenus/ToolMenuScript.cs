using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolMenuScript : GameMenu {
	void Awake() {
		menuName = "ToolMenu";
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.BackQuote)) {
			if (MenuController.Instance.GameIsPausedByMenu(menuName)) {
				DeactiveMenu();
			} else {
				ActivateMenu();
			}
		}
	}
}