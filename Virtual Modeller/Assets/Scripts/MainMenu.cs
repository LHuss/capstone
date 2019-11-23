using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleFileBrowser;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitApplication ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void OpenExplorer()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    public void LoadScene (string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

        // Dialog is closed
        // Print whether a file is chosen (FileBrowser.Success)
        // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);

        if (FileBrowser.Success)
        {
            // If a file was chosen, read its bytes via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
        }
    }

}
