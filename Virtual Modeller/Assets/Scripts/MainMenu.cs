using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public MainMenuController mainMenuController;
    public GameObject loginMenu;
    public GameObject optionsMenu;
    public GameObject mainMenu;
    private bool show;

    public void Awake()
    {
        loginMenu = GameObject.Find("LoginMenu");
        optionsMenu = GameObject.Find("OptionsMenu");
        mainMenu = GameObject.Find("MainMenu");
        show = true;
        ShowMainMenu();
    }

    public void Start()
    {  
    }

    public void ShowMainMenu()
    {
        //this.mainMenuController.ActivateMainMenu(this.mainMenu, this.optionsMenu, this.loginMenu);
        mainMenu.SetActive(show);
        loginMenu.SetActive(!show);
        optionsMenu.SetActive(!show);
    }

    public void ShowOptionsMenu()
    {
        //this.mainMenuController.ActivateMainMenu(this.mainMenu, this.optionsMenu, this.loginMenu);
        mainMenu.SetActive(!show);
        loginMenu.SetActive(!show);
        optionsMenu.SetActive(show);
    }

    public void ShowLoginMenu()
    {
        //this.mainMenuController.ActivateLoginMenu(this.mainMenu, this.optionsMenu, this.loginMenu);
        mainMenu.SetActive(!show);
        loginMenu.SetActive(show);
        optionsMenu.SetActive(!show);
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
