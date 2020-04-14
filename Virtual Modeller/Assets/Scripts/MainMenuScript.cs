using System.Collections;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void QuitApplication()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
