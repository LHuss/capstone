using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        
        // scale down
        if (Input.GetKey(",")) {
            string currentScene = SceneManager.GetActiveScene().name;
            GameObject gameObj;
            GameObject gameModel;
            if (currentScene == "CreationScene")
            {
                gameModel = GameObject.Find("HugeSphere");
            }
            else {
                gameObj = GameObject.Find("My Object");
                gameModel = gameObj.transform.GetChild(0).gameObject;
            }

            Vector3 objScale = gameModel.transform.localScale;
            if (objScale == new Vector3(0.1f, 0.1f, 0.1f)) {
                return;
            }
            else {
                gameModel.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            }
        }

        // scale up
        if (Input.GetKey("."))
        {
            string currentScene = SceneManager.GetActiveScene().name;
            GameObject gameObj;
            GameObject gameModel;
            if (currentScene == "CreationScene")
            {
                gameModel = GameObject.Find("HugeSphere");
            }
            else
            {
                gameObj = GameObject.Find("My Object");
                gameModel = gameObj.transform.GetChild(0).gameObject;
            }
            gameModel.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
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
