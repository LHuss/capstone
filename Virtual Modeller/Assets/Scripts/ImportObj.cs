using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using UnityEngine;
using SFB; // StandaloneFileBrowser

public class ImportObj : MonoBehaviour {

	private string fileDir = "";

	public void showDialog(){

        Debug.Log("0000000000000");	
		ExtensionFilter[] extensionList = new [] {
                new ExtensionFilter("Waveform obj", "obj")
            };
		string[] path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensionList, false);

		fileResult(path);
		loadModel(fileDir);
	}

	public void fileResult(string[] p) {
        if (p.Length == 0) {
        	Debug.Log("file path len = 0");
        }
        else{
        	foreach (string s in p){
        		fileDir += s;
        	}
        	Debug.Log(fileDir);
        }
    }

    /**
    *	To-do: mesh-rendering and probably more
    */
    public void loadModel(string s){
    	Mesh mesh = FastObjImporter.Instance.ImportFile(s);
    	Material material = new Material(Shader.Find("Standard")) {
    		color = new Color(0,0,255)
    	};

    	GameObject gameObj = new GameObject("Model");

    }

/*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	*/
}
