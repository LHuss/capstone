using UnityEngine;
using SFB; // StandaloneFileBrowser
using AsImpL;
using Leap.Unity.Interaction;

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
        importOptions.modelScaling = 1f;
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<ObjectImporter>();
        ObjectImporter objImporter = gameObject.GetComponent<ObjectImporter>();

        Debug.Log(fp);
        Debug.Log(filePath);
        if (filePath=="") {
            filePath = fp;
        }
        Debug.Log(filePath);
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
        GameObject Go = importedObject.transform.GetChild(0).gameObject;
        Go.AddComponent<MeshCollider>();
        Go.AddComponent<MeshController>();
        Go.AddComponent<ObjectMovement>();
        Go.AddComponent<InteractionBehaviour>();
        Rigidbody rigidbody = Go.GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void PositionObject(GameObject importedObject)
    {
        importedObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        importedObject.transform.Translate(0f, 0f, 0f);
    }

}