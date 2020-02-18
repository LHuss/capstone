using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenScript : GameMenu {
	void Awake() {
		menuName = "Loading";
		MenuController.Instance.AddStaticMenu(StaticMenuType.LOADING_MENU, this);
	}
}
