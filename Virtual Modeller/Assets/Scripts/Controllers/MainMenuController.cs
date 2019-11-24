using UnityEngine;

public class MainMenuController : MonoBehaviour {

    public void ActivateMainMenu(GameObject mainMenu, GameObject optionsMenu, GameObject loginMenu)
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        loginMenu.SetActive(false);
    }

    public void ActivateOptionsMenu(GameObject mainMenu, GameObject optionsMenu, GameObject loginMenu)
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        loginMenu.SetActive(false);
    }

    public void ActivateLoginMenu(GameObject mainMenu, GameObject optionsMenu, GameObject loginMenu)
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        loginMenu.SetActive(true);
    }

}
