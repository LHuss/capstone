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

public class ImportFromCloud : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string currLocalDir = Directory.GetCurrentDirectory();
        string default_cred_path = currLocalDir + "\\library\\stately-mote-267221-0b07ba79bf7a.json";
        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", default_cred_path);
    }

    public void OnClick() {

        // get list of files in cloud
        List<Google.Apis.Storage.v1.Data.Object> fileList = GetFileList();

        // display the files to the user
        DisplayFiles(fileList);
    }

    // to do
    public void DisplayFiles(List<Google.Apis.Storage.v1.Data.Object> flist) {

        // show the files in a dialog

        // import the file selected by the user
        //DownloadFile(something, something, something);

    }

    // functional
    public void DownloadFile(string fileToDownload, StorageClient storage, string bucketName) {
        string objectName = fileToDownload;
        string localPath = Path.GetFileName(objectName); // by default, save the file to the virtual modeller's root directory, will change later
        using (var outputFile = File.OpenWrite(localPath))
        {
            storage.DownloadObject(bucketName, objectName, outputFile);
        }
        Debug.Log($"downloaded {objectName} to {localPath}.");
    }

    // functional
    public List<Google.Apis.Storage.v1.Data.Object> GetFileList() {

        List<Google.Apis.Storage.v1.Data.Object> filesInCloud = new List<Google.Apis.Storage.v1.Data.Object>();
                
        string currLocalDir = Directory.GetCurrentDirectory();

        string projectId = "stately-mote-267221";
        string bucketName = "vmod_bucket";
        
        /*
        
        string secretId = "vmod_sec";
        string secretVersion = "latest";        

        // Create the request.
        var request = new AccessSecretVersionRequest
        {
            SecretVersionName = new SecretVersionName(projectId, secretId, secretVersion),
        };

        SecretManagerServiceClient client = SecretManagerServiceClient.Create();

        var version = client.AccessSecretVersion(request);
        string payload = version.Payload.Data.ToStringUtf8();
        Debug.Log($"Payload: {payload}");
        */          
        
        string credentialsJson = "";
        
        using (StreamReader r = new StreamReader(currLocalDir + "\\library\\stately-mote-267221-0b07ba79bf7a.json"))
        {
            credentialsJson = r.ReadToEnd();
        }

        GoogleCredential cred = GoogleCredential.FromJson(credentialsJson);
        StorageClient storage = StorageClient.Create(cred);

        Debug.Log(storage.Service);

        ServicePointManager.ServerCertificateValidationCallback += (System.Object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors) => true;

        var storageObject = storage.ListObjects(bucketName, "");
        foreach (var stObj in storageObject){
            filesInCloud.Add(stObj);
            Debug.Log($"found {stObj.Name}");
        }
        

        // This part is only here temporarily while I test if download works
        string objectName = "teapot.obj";
        string localPath = Directory.GetCurrentDirectory() + "\\" + Path.GetFileName(objectName); // by default, save the file to the virtual modeller's root directory, will change later
        using (var outputFile = File.OpenWrite(localPath))
        {
            storage.DownloadObject(bucketName, objectName, outputFile);
        }
        Debug.Log($"downloaded {objectName} to {localPath}.");   
        // =============


        return filesInCloud;
    }	

	// Update is called once per frame
	void Update () {
		
	}
}
