using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : Singleton<MenuController> {
	private HashSet<string> activeMenus;

	public bool GameIsPausedByMenu(string menu) {
		return activeMenus.Contains(menu);
	}

	private void Awake() {
		activeMenus = new HashSet<string>();
	}

	public void Pause(string menu) {
		Time.timeScale = 0f;
		activeMenus.Add(menu);
		Debug.Log("Game Paused by " + menu);
	}

	public void TryResume(string menu) {
		activeMenus.Remove(menu);
		if (activeMenus.Count == 0) {
			Time.timeScale = 1f;
			Debug.Log("Game Resumed by " + menu);
		} else {
			Debug.Log("Game resume attempted by " + menu);
			string activeMenuNames = "[" + string.Join(", ", activeMenus) + "]" ;
			Debug.Log("Game still paused by following menu(s): " + activeMenuNames);
		}
	}
}
