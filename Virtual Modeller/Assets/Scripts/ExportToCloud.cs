using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System;

public class ExportToCloud : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string currLocalDir = Directory.GetCurrentDirectory();
		string default_cred_path = currLocalDir + "\\library\\stately-mote-267221-0b07ba79bf7a.json";
        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", default_cred_path);
	}

	public void OnClick() {
		ExportFileToCloud();
    }
	
	public void ExportFileToCloud() {

		// first, save the model to a file
        ExportObj eobj = new ExportObj();
        eobj.SaveFile();
        string filePath = eobj.SavedFilePath;
        string fileName = eobj.SavedFileName;

        // then, export to cloud
        string bucketName = "vmod_bucket";
		string currLocalDir = Directory.GetCurrentDirectory();
		string credentialsJson = "";
        
        using (StreamReader r = new StreamReader(currLocalDir + "\\library\\stately-mote-267221-0b07ba79bf7a.json"))
        {
            credentialsJson = r.ReadToEnd();
        }

        GoogleCredential cred = GoogleCredential.FromJson(credentialsJson);
        StorageClient storage = StorageClient.Create(cred);

        Debug.Log(storage.Service);

        ServicePointManager.ServerCertificateValidationCallback += (System.Object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors) => true;

        using (var f = File.OpenRead(filePath)) {

	        storage.UploadObject(bucketName, fileName, null, f);
	        Console.WriteLine($"Uploaded {fileName}.");
	    }

	}
}
