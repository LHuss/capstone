using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using UnityEngine;
using SFB; // StandaloneFileBrowser

public class ImportObj : MonoBehaviour {

	private string fileDir = "";

	public void init(){

		Camera cam = Camera.main;

		Debug.Log("0000000000000");	
		ExtensionFilter[] extensionList = new [] {
                new ExtensionFilter("Waveform obj", "obj")
            };
		string[] path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensionList, false);

		fileResult(path);
		GameObject gameObject = loadModel(fileDir);

		gameObject.transform.position = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));

	}

	public void fileResult(string[] p) {
        if (p.Length == 0) {
        	Debug.Log("file path len = 0");
        }
        else{
        	foreach (string s in p){
        		fileDir = p[0];
        	}
        	Debug.Log(fileDir);
        }
    }

    /**
    *	To-do: mesh-rendering and probably more
    */
    public GameObject loadModel(string s){
    	Debug.Log(s);
    	Mesh mesh1 = FastObjImporter.Instance.ImportFile(s);
    	Material mat1 = new Material(Shader.Find("Standard"));

    	GameObject gameObj = new GameObject("Model");
    	
		MeshFilter meshFilter = gameObj.AddComponent<MeshFilter>();
    	MeshRenderer meshRenderer = gameObj.AddComponent<MeshRenderer>();
    	meshFilter.mesh = mesh1;
    	meshRenderer.material = mat1;
    	
    	return gameObj;
    }
/*
	// Update is called once per frame
	void Update () {
		
	}
	*/
}
