using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : Singleton<MenuController> {

	private bool gameIsPaused;

	public bool GameIsPaused {
		get {
			return gameIsPaused;
		}
	}

	public void Pause() {
		Time.timeScale = 0f;
		this.gameIsPaused = true;
	}

	public void Resume() {
		Time.timeScale = 1f;
		this.gameIsPaused = false;
	}
}
