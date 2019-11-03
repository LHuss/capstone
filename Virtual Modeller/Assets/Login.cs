using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Login : MonoBehaviour
{

    //Hardcoded Admin Credentials
    private string u = "admin";
    private string p = "admin";

    public InputField usernameString;
    public InputField passwordString;
    public Button loginBtn;
    public Button profileBtn;
    public CanvasGroup mainMenu;
    public CanvasGroup loginMenu;


    public void CheckLogin ()
    {
        
        if(u.Equals(usernameString.text)&&p.Equals(passwordString.text))
        {
            loginBtn.enabled = false;
            loginMenu.enabled = false;
            profileBtn.enabled = true;
            mainMenu.enabled = true;
            Debug.Log("Success, user " + usernameString + " is now logged in.");
        }
        else
        {
            Debug.Log(usernameString.text);
            Debug.Log(passwordString.text);
            Debug.Log("Wrong username or password, please try again.");
        }

    }



}
