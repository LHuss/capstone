using System.Collections;
using System.Collections.Generic;
using SFB; // StandaloneFileBrowser
using UnityEngine;

public class ExportObj : MonoBehaviour {

    public void SaveFile()
    {
        MeshFilter[] filters = FindObjectsOfType<MeshFilter>();
        Debug.Log(filters.Length);
        ExtensionFilter[] extensionList = new[] {
            new ExtensionFilter("Waveform obj", "obj")
        };
        var path = StandaloneFileBrowser.SaveFilePanel("Export OBJ file", "", "ExportedFile", extensionList);
    }
}
