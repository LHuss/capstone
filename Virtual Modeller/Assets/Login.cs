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
	public GameObject loginBtn;
	public GameObject loginMenu;
	public GameObject mainMenu;
	public Text welcomeMessage;


    public void CheckLogin ()
    {
        
        if(u.Equals(usernameString.text)&&p.Equals(passwordString.text))
        {
			/*
            loginBtn.enabled = false;
            loginMenu.enabled = false;
            profileBtn.enabled = true;
            mainMenu.enabled = true;
            */
            Debug.Log("Success, user " + usernameString + " is now logged in.");
			loginMenu.SetActive (false);
			mainMenu.SetActive (true);
			loginBtn.SetActive (false);
			welcomeMessage.text = "Welcome back " + usernameString + "!";
			/*
            Debug.Log("login btn :"+loginBtn.enabled);
            Debug.Log("login menu ::"+loginMenu.enabled);
            Debug.Log("profile btn :" +profileBtn.enabled);
            Debug.Log("main menu :"+mainMenu.enabled);
            */
        }
        else
        {
            Debug.Log(usernameString.text);
            Debug.Log(passwordString.text);
            Debug.Log("Wrong username or password, please try again.");
		}

    }



}
