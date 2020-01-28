using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ImportObjTests {
    private GameObject SetupGameObject() {
        GameObject parent = new GameObject();
        GameObject child = new GameObject();
        child.transform.parent = parent.transform;

        return parent;
    }

	[UnityTest]
	public IEnumerator _Check_Imported_Object_Has_MeshController_Component() {
        var importObj = new GameObject().AddComponent<ImportObj>();
        GameObject imported = SetupGameObject();
		yield return null;
        importObj.AttachComponents(imported);
        GameObject go = imported.transform.GetChild(0).gameObject;
        Assert.NotNull(go.GetComponent<MeshController>());
    }

    [UnityTest]
    public IEnumerator _Check_Imported_Object_Has_MeshCollider_Component()
    {
        var importObj = new GameObject().AddComponent<ImportObj>();
        GameObject imported = SetupGameObject();
		yield return null;
        importObj.AttachComponents(imported);
        GameObject go = imported.transform.GetChild(0).gameObject;
        Assert.NotNull(go.GetComponent<MeshCollider>());
    }

    [UnityTest]
    public IEnumerator _Check_Imported_Object_Has_ObjectMovement_Component()
    {
        var importObj = new GameObject().AddComponent<ImportObj>();
        GameObject imported = SetupGameObject();
		yield return null;
        importObj.AttachComponents(imported);
        GameObject go = imported.transform.GetChild(0).gameObject;
        Assert.NotNull(go.GetComponent<ObjectMovement>());
    }

    [UnityTest]
    public IEnumerator _Check_Imported_Object_Has_Rigibody_Component()
    {
        var importObj = new GameObject().AddComponent<ImportObj>();
        GameObject imported = SetupGameObject();
		yield return null;
        importObj.AttachComponents(imported);
        GameObject go = imported.transform.GetChild(0).gameObject;
        Assert.NotNull(go.GetComponent<Rigidbody>());
    }

    [UnityTest]
    public IEnumerator _Check_Imported_Object_Collision_Detection_ON()
    {
        var importObj = new GameObject().AddComponent<ImportObj>();
        GameObject imported = SetupGameObject();
		yield return null;
        importObj.AttachComponents(imported);
        GameObject go = imported.transform.GetChild(0).gameObject;
        Rigidbody rb = go.GetComponent<Rigidbody>();
        Assert.AreEqual(true,rb.detectCollisions);
    }
}
