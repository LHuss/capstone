﻿using System.Collections;
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
		string default_cred_path = currLocalDir + "\\Assets\\ThirdParty\\stately-mote-267221-0b07ba79bf7a.json";
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

        // hard-coding the gcloud credentials since the standalone exe won't be able to find the json file containing this
        string credentialsJson = @"{
                          'type': 'service_account',
                          'project_id': 'stately-mote-267221',
                          'private_key_id': '0b07ba79bf7ac0e0cb2e224958dc260817056302',
                          'private_key': '-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCb6ol5BGiy20Ag\n2nqaeYBhgEVfFUqZ1jW8wM+merA5bm98nZRbvxoH9ZsZ7J2HM4TcuB8vXZLsoOoR\nbPpqRLlAzhBvRrdlG5g3+SKKrV0IIkofrS8hWI5OAwzNcPi/1BpzzguAiU4On+N2\n03gjq3ZyuvwF55/2Y60EE7XSbR6w5KKvaeIXSIsbbKF6Y3vFipFfvnr9ZMQ73Zxp\nil6Lc/UB/thuvMw4vRrCqxjqv1S8LuzGJhriUGytxjg7z9RbO/F+vEM2mhZ6G5ZE\nSbNSACA0VhWUwHterE0XQ+hm0GDoiYKzSZrVKAZ5F6NtfHz1hT4fD9eeXTcyuEgb\nCiwadMWfAgMBAAECggEADnal1Q+XJMxY+RM7X1wZEDJlfQoNFxM7feyG9sMX2OQJ\nPI6jphUmtYlPn4qHHHUnhJAh+HNeP0w4GnC5FCWW58+Vz7TIK31xU/PtA99MK4Z+\nyMx4fEfqP4th4ybxNF5ZK9f30i8qignEAJBiazCrNr5oxaV0fddGRAQOEFymBbOy\nuKCBj/7qBAksKrJLnlwOO+CETZc7UygxEdTdhRJWaKDzBq97LdFgbgyx/n+RFNwW\nk5SFWNtrt+Ah0/nHvgdGi+nJnYAdk/zi7gRaqjbOzm8BpSqSM0Xz7vdBuBfoEjY2\nOWauVOiRErjqskd+Ny0m6QfQABX4Jwk+2FsyLKXO/QKBgQDbAiNepZOVwBfop3Cw\nFfZmjeHgF75PmkefA7kJNMiFAoU97ksOLWXYVwgi+hnbIrZLC6FG70c/KIMF0j1x\nNuIeBflqcNlQs+b/8srTBMwMiYNY11N34Osn3aHMCphwtBOs15wQCyh3troGwhJD\nNsiq/aZGtHtyPb+NJ3cLYsy4cwKBgQC2QE8+N6PR1xSfu5RlSr36L+FFhGCp93Hk\nowaF3xujE8cDjq0/aSghHZUwdodM/Qe4SrHt74Isj4Wy9z34nW8LszGnmTE4eVvF\n15QyU5WD+21bcMH+olyPXRSuS1lCq31gI29moovy7HmN9yO2+CLqlSjtiTj80FzD\nuhLSO7svJQKBgQCISjxYmhHOD0zRq+GkswYzq/f0zNHSF+CaRGbSI9blbzwb3j83\nA2ltyDt4CMwuYtuut/4VdrKPy1Y+OSejXNQ6et1MMA4M+ue2QBGYYFPbOXhTwSxg\nXcf5dyNJJw7WlDnqRgMIuOjmFwCNVBipW15lipP/TDHGVkbuQLEElOdxoQKBgG+J\nM/Xzv0IwyuJmvg6vi0yN+OO+fBoI4Z73VoqfXB6Vf/phWw2voWuC20bpgyxOvma+\n792Z8qSqwTwhq793OfqDFCRp0IGrY8rUgFG8bYh0WxzXCSJ44wSqBnoUivAOW5B2\nnzrEx7lHl6yWTzku6s99saqNjF7MdbkjK1mWTJwxAoGBAIrMqozIdOoAfKxFIDMT\nqoYzamo2q1YMhVbZl++n0tmllv4xQnUv3VUgOlrCR3I6XO/qOHbIiEm3oOuzWDvX\n+uMmbI+7pxUq5M+9MSzvNlVwcVJtBO2UMwdluSBXOtw7lqNp5MZcsdSiNerk6YvQ\n30N+djRrGFeVyre1mGbJCigo\n-----END PRIVATE KEY-----\n',
                          'client_email': 'conu-vmod@stately-mote-267221.iam.gserviceaccount.com',
                          'client_id': '101714657942361325319',
                          'auth_uri': 'https://accounts.google.com/o/oauth2/auth',
                          'token_uri': 'https://oauth2.googleapis.com/token',
                          'auth_provider_x509_cert_url': 'https://www.googleapis.com/oauth2/v1/certs',
                          'client_x509_cert_url': 'https://www.googleapis.com/robot/v1/metadata/x509/conu-vmod%40stately-mote-267221.iam.gserviceaccount.com'
                        }";

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
