using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using UnityEngine;
using SFB; // StandaloneFileBrowser
using AsImpL;

public class ImportObj : MonoBehaviour {

	private string[] path;
	private string filePath = "";

	public void OpenFile(){

		ExtensionFilter[] extensionList = new [] {
                new ExtensionFilter("Waveform obj", "obj")
            };
		path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensionList, false);
		fileResult(path);

		if (filePath.Length!=0){
			importObjFromFile(filePath);
		}
		else{
			Debug.Log("Obj file not selected");
		}
		
	}

	public void importObjFromFile(string fp){
		Camera cam = Camera.main;

		GameObject gameObject = new GameObject("Mesh");
		gameObject.AddComponent<Rigidbody>();
		
		Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

		ObjectImporter objImporter = gameObject.GetComponent<ObjectImporter>();
		ImportOptions importOptions = new ImportOptions();
		importOptions.buildColliders = true;
		importOptions.modelScaling = 2f;
		objImporter = gameObject.AddComponent<ObjectImporter>();			

		//rigidbody.isKinematic = true;
		rigidbody.detectCollisions = true;
		rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

		objImporter.ImportModelAsync("My Object", filePath, null, importOptions);

		gameObject.transform.position = cam.ScreenToWorldPoint(new Vector3(90f, 160f, -50f));

		Debug.Log("Are kinematics enabled?: " + rigidbody.isKinematic);
		Debug.Log("Are collision detections enabled?: " + rigidbody.detectCollisions);
		Debug.Log("Current collision detection mode: " + rigidbody.collisionDetectionMode);
	}

	public void fileResult(string[] p) {
        if (p.Length == 0) {
        	Debug.Log("file path len = 0");
        }
        else{
        	filePath = p[0];
        	Debug.Log(filePath);
        }
    }

}
