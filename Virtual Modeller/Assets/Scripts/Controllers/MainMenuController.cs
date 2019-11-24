using UnityEngine;

public class MainMenuController : MonoBehaviour {

    private readonly bool showMenu = true;

    public void ActivateMainMenu(GameObject mainMenu, GameObject optionsMenu, GameObject loginMenu)
    {
        mainMenu.SetActive(showMenu);
        loginMenu.SetActive(!showMenu);
        optionsMenu.SetActive(!showMenu);
    }

    public void ActivateOptionsMenu(GameObject mainMenu, GameObject optionsMenu, GameObject loginMenu)
    {
        mainMenu.SetActive(!showMenu);
        loginMenu.SetActive(!showMenu);
        optionsMenu.SetActive(showMenu);
    }

    public void ActivateLoginMenu(GameObject mainMenu, GameObject optionsMenu, GameObject loginMenu)
    {
        mainMenu.SetActive(!showMenu);
        loginMenu.SetActive(showMenu);
        optionsMenu.SetActive(!showMenu);
    }

}
