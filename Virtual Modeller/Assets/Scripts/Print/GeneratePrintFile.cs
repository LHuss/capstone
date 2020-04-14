using UnityEngine;
using SFB; // StandaloneFileBrowser
using System.Diagnostics;
using System;

public class GeneratePrintFile : MonoBehaviour
{
    private string[] path;
    public void SelectFile()
    {
        ExtensionFilter[] extensionList = new[] {
                new ExtensionFilter("Waveform obj", "obj"),
                new ExtensionFilter("Standard Tessellation Language", "stl")
            };
        path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensionList, false);
        if (path[0].Length != 0)
        {   
            UnityEngine.Debug.Log("Generating GCODE");
            GenerateGcode(path[0]);
        }
        else
        {
            UnityEngine.Debug.Log("Obj file not selected");
        }
    }

    public void GenerateGcode(string filePath){
        UnityEngine.Debug.Log(filePath);
        var outputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        UnityEngine.Debug.Log(outputPath);
        var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "Slic3r_print.sh",
                WorkingDirectory = Application.dataPath + "/Scripts/Print/",
                Arguments = $"\"{outputPath}\" \"{filePath}\"",
                UseShellExecute = true,
                CreateNoWindow = false
            }
        };
        
        proc.Start();
        proc.WaitForExit();
    }
}