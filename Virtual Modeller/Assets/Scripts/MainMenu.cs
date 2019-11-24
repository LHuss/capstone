using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private MainMenuController mainMenuController;
    public GameObject loginMenu;
    public GameObject optionsMenu;
    public GameObject mainMenu;

    public void Awake()
    {
        mainMenuController = GetComponent<MainMenuController>();
    }

    public void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuController.ActivateMainMenu(mainMenu, optionsMenu, loginMenu);
    }

    public void ShowOptionsMenu()
    {
        mainMenuController.ActivateOptionsMenu(mainMenu, optionsMenu, loginMenu);
    }

    public void ShowLoginMenu()
    {
        mainMenuController.ActivateLoginMenu(mainMenu, optionsMenu, loginMenu);
    }

    public void LoadScene (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

}
