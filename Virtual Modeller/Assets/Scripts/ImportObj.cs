using UnityEngine;
using SFB; // StandaloneFileBrowser
using AsImpL;

public class ImportObj : MonoBehaviour
{

    private string[] path;
    private string filePath = "";

    public void OpenFile()
    {
        ExtensionFilter[] extensionList = new[] {
                new ExtensionFilter("Waveform obj", "obj")
            };
        path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensionList, false);
        Debug.Log(path);
        FileResult(path);
        if (filePath.Length != 0)
        {
            ImportObjFromFile(filePath);
        }
        else
        {
            Debug.Log("Obj file not selected");
        }
    }

    public void ImportObjFromFile(string fp)
    {
        Camera cam = Camera.main;
        ImportOptions importOptions = new ImportOptions();
        importOptions.buildColliders = true;
        importOptions.modelScaling = 1f;
        gameObject.AddComponent<ObjectImporter>();
        ObjectImporter objImporter = gameObject.GetComponent<ObjectImporter>();
        objImporter.ImportModelAsync("My Object", filePath, null, importOptions);
        objImporter.ImportedModel += (GameObject importedObject, string path) =>
        {
            //manipulate the asynchronously imported object
            AttachComponents(importedObject);
            PositionObject(importedObject);
        };
    }

    public void FileResult(string[] p)
    {
        if (p.Length == 0)
        {
            Debug.Log("file path len = 0");
        }
        else
        {
            filePath = p[0];
            Debug.Log(filePath);
        }
    }

    public void AttachComponents(GameObject importedObject)
    {
        importedObject.AddComponent<MeshFilter>();
        importedObject.AddComponent<MeshCollider>();
        MeshCollider objMeshCollider = importedObject.GetComponent<MeshCollider>();
        importedObject.AddComponent<Model>();
        importedObject.AddComponent<Rigidbody>();
        Rigidbody rigidbody = importedObject.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void PositionObject(GameObject importedObject)
    {
        importedObject.transform.localScale = new Vector3(10f, 10f, 10f);
        importedObject.transform.Translate(0f, 0f, 0f);
    }

}