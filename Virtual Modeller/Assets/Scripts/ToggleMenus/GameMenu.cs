using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {
	private string _menuName;
	public string menuName {
		get {
			return _menuName;
		}
		protected set {
			_menuName = value;
		}
	}

	public GameObject menuUI;

	public void ActivateMenu() {
		menuUI.SetActive(true);
		MenuController.Instance.Pause(menuName);
	}

	public void DeactiveMenu() {
		menuUI.SetActive(false);
		MenuController.Instance.TryResume(menuName);
	}
}
