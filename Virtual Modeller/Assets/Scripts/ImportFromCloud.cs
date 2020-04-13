using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Windows.Forms;
using System.Drawing;
using System;
using TMPro;

public class ImportFromCloud : MonoBehaviour {

    public GameObject Panel;
    public GameObject ScrollView;
    public TextMeshProUGUI txtMeshObj;

    private StorageClient storage;
    private string bucketName;


    // Use this for initialization
    void Start () {
        string currLocalDir = Directory.GetCurrentDirectory();
        string default_cred_path = currLocalDir + "\\Assets\\ThirdParty\\stately-mote-267221-0b07ba79bf7a.json";
        System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", default_cred_path);
    }

    public void OnClick() {
                
        // get list of files in cloud
        List<Google.Apis.Storage.v1.Data.Object> fileList = GetFileList();

        // display the files to the user
        DisplayFiles(fileList);
    }

    public void OnExit() {
        if (ScrollView != null && ScrollView.active == true)
        {
            ScrollView.SetActive(false);
        }
    }

    public void DisplayFiles(List<Google.Apis.Storage.v1.Data.Object> flist) {
        string allFiles = "";
        foreach (var file in flist)
        {
            int lastIndexOfDot = file.Name.LastIndexOf('.');
            string name = file.Name.Substring(0, lastIndexOfDot);
            string ext = file.Name.Substring(lastIndexOfDot + 1);
            Debug.Log($"found file named {name} with ext {ext}.");
            if (ext == "obj".ToLower()){
                allFiles += name + "\n";
            }
        }

        // display panel
        if (ScrollView != null && ScrollView.active == false)
        {
            ScrollView.SetActive(true);
        }
        txtMeshObj = Panel.GetComponentInChildren<TextMeshProUGUI>();
        txtMeshObj.text = allFiles;      

    }

    public void DownloadFile(string fileToDownload, StorageClient sc, string bn) {
        string objectName = fileToDownload;
        string localPath = Directory.GetCurrentDirectory() + "\\" + Path.GetFileName(objectName);
        using (var outputFile = File.OpenWrite(localPath))
        {
            sc.DownloadObject(bn, objectName, outputFile);
        }
        Debug.Log($"downloaded {objectName} to {localPath}.");
        ScrollView.SetActive(false);
    }
    
    public List<Google.Apis.Storage.v1.Data.Object> GetFileList() {

        List<Google.Apis.Storage.v1.Data.Object> filesInCloud = new List<Google.Apis.Storage.v1.Data.Object>();
                
        string currLocalDir = Directory.GetCurrentDirectory();

        string projectId = "stately-mote-267221";
        bucketName = "vmod_bucket";
         
        string credentialsJson = "";
        
        using (StreamReader r = new StreamReader(currLocalDir + "\\Assets\\ThirdParty\\stately-mote-267221-0b07ba79bf7a.json"))
        {
            credentialsJson = r.ReadToEnd();
        }

        GoogleCredential cred = GoogleCredential.FromJson(credentialsJson);
        storage = StorageClient.Create(cred);

        Debug.Log(storage.Service);

        ServicePointManager.ServerCertificateValidationCallback += (System.Object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors) => true;

        var storageObject = storage.ListObjects(bucketName, "");
        foreach (var stObj in storageObject){
            filesInCloud.Add(stObj);
            //Debug.Log($"found {stObj.Name}");
        }
        
        return filesInCloud;
    }

    public void Update(){
        if (ScrollView.active == true){
            if (Input.GetMouseButtonDown(0)){
                var wordIndex = TMP_TextUtilities.FindIntersectingWord(txtMeshObj, Input.mousePosition, null);
                string selectedFile;
                if (wordIndex != -1){
                    selectedFile = txtMeshObj.textInfo.wordInfo[wordIndex].GetWord() + ".obj";
                    DownloadFile(selectedFile, storage, bucketName);
                }
            }
        }
    }

}
