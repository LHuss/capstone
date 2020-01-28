using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
    //Hardcoded Admin Credentials
    private string u = "admin";
    private string p = "admin";
    public InputField usernameString;
    public InputField passwordString;
    public GameObject loginBtn;
    public GameObject loginMenu;
    public GameObject mainMenu;
    public Text welcomeMessage;
    public GameObject loginPopup;

    public void CheckLogin()
    {
        if (u.Equals(usernameString.text) && p.Equals(passwordString.text))
        {
            Debug.Log("Success, user " + usernameString + " is now logged in.");
            loginMenu.SetActive(false);
            mainMenu.SetActive(true);
            loginBtn.SetActive(false);
            welcomeMessage.text = "Welcome back " + usernameString + "!";
        }
        else
        {
            loginPopup.SetActive(true);
            Debug.Log(usernameString.text);
            Debug.Log(passwordString.text);
            Debug.Log("Wrong username or password, please try again.");
        }
    }

}
