using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuScript : GameMenu {
	void Awake() {
		menuName = "ImportMenu";
		ActivateMenu();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (MenuController.Instance.GameIsPausedByMenu(menuName)) {
				DeactiveMenu();
			} else {
				ActivateMenu();
			}
		}
	}
}
