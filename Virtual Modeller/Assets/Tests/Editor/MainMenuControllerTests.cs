using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class MainMenuControllerTests {
    [UnityTest]
    public IEnumerator _Check_Main_When_Main_Menu_Active()
    {
        var mainMenuController = new GameObject().AddComponent<MainMenuController>();
        var mainMenu = new GameObject();
        var loginMenu = new GameObject();
        var optionsMenu = new GameObject();
        yield return null;
        mainMenuController.ActivateMainMenu(mainMenu, optionsMenu, loginMenu);
        Assert.AreEqual(true, mainMenu.activeSelf);
    }

    [UnityTest]
    public IEnumerator _Check_Login_When_Main_Menu_Active()
    {
        var mainMenuController = new GameObject().AddComponent<MainMenuController>();
        var mainMenu = new GameObject();
        var loginMenu = new GameObject();
        var optionsMenu = new GameObject();
        yield return null;
        mainMenuController.ActivateMainMenu(mainMenu, optionsMenu, loginMenu);
        Assert.AreEqual(false, loginMenu.activeSelf);
    }

    [UnityTest]
    public IEnumerator _Check_Options_When_Main_Menu_Active()
    {
        var mainMenuController = new GameObject().AddComponent<MainMenuController>();
        var mainMenu = new GameObject();
        var loginMenu = new GameObject();
        var optionsMenu = new GameObject();
        yield return null;
        mainMenuController.ActivateMainMenu(mainMenu, optionsMenu, loginMenu);
        Assert.AreEqual(false, optionsMenu.activeSelf);
    }

    [UnityTest]
    public IEnumerator _Check_Main_When_Login_Menu_Active()
    {
        var mainMenuController = new GameObject().AddComponent<MainMenuController>();
        var mainMenu = new GameObject();
        var loginMenu = new GameObject();
        var optionsMenu = new GameObject();
        yield return null;
        mainMenuController.ActivateLoginMenu(mainMenu, optionsMenu, loginMenu);
        Assert.AreEqual(false, mainMenu.activeSelf);
    }

    [UnityTest]
    public IEnumerator _Check_Login_When_Login_Menu_Active()
    {
        var mainMenuController = new GameObject().AddComponent<MainMenuController>();
        var mainMenu = new GameObject();
        var loginMenu = new GameObject();
        var optionsMenu = new GameObject();
        yield return null;
        mainMenuController.ActivateLoginMenu(mainMenu, optionsMenu, loginMenu);
        Assert.AreEqual(true, loginMenu.activeSelf);
    }

    [UnityTest]
    public IEnumerator _Check_Options_When_Login_Menu_Active()
    {
        var mainMenuController = new GameObject().AddComponent<MainMenuController>();
        var mainMenu = new GameObject();
        var loginMenu = new GameObject();
        var optionsMenu = new GameObject();
        yield return null;
        mainMenuController.ActivateLoginMenu(mainMenu, optionsMenu, loginMenu);
        Assert.AreEqual(false, optionsMenu.activeSelf);
    }

    [UnityTest]
    public IEnumerator _Check_Main_When_Options_Menu_Active()
    {
        var mainMenuController = new GameObject().AddComponent<MainMenuController>();
        var mainMenu = new GameObject();
        var loginMenu = new GameObject();
        var optionsMenu = new GameObject();
        yield return null;
        mainMenuController.ActivateOptionsMenu(mainMenu, optionsMenu, loginMenu);
        Assert.AreEqual(false, mainMenu.activeSelf);
    }

    [UnityTest]
    public IEnumerator _Check_Login_When_Options_Menu_Active()
    {
        var mainMenuController = new GameObject().AddComponent<MainMenuController>();
        var mainMenu = new GameObject();
        var loginMenu = new GameObject();
        var optionsMenu = new GameObject();
        yield return null;
        mainMenuController.ActivateOptionsMenu(mainMenu, optionsMenu, loginMenu);
        Assert.AreEqual(false, loginMenu.activeSelf);
    }

    [UnityTest]
    public IEnumerator _Check_Options_When_Options_Menu_Active()
    {
        var mainMenuController = new GameObject().AddComponent<MainMenuController>();
        var mainMenu = new GameObject();
        var loginMenu = new GameObject();
        var optionsMenu = new GameObject();
        yield return null;
        mainMenuController.ActivateOptionsMenu(mainMenu, optionsMenu, loginMenu);
        Assert.AreEqual(true, optionsMenu.activeSelf);
    }
}
