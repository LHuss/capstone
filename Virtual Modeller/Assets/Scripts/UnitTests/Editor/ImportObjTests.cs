using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ImportObjTests {

	[UnityTest]
	public IEnumerator _Check_Imported_Object_Has_MeshFilter_Component() {
        var importObj = new GameObject().AddComponent<ImportObj>();
        GameObject go = new GameObject();
		yield return null;
        importObj.AttachComponents(go);
        Assert.NotNull(go.GetComponent<MeshFilter>());
    }

    [UnityTest]
    public IEnumerator _Check_Imported_Object_Has_MeshCollider_Component()
    {
        var importObj = new GameObject().AddComponent<ImportObj>();
        GameObject go = new GameObject();
        yield return null;
        importObj.AttachComponents(go);
        Assert.NotNull(go.GetComponent<MeshCollider>());
    }

    [UnityTest]
    public IEnumerator _Check_Imported_Object_Has_Model_Component()
    {
        var importObj = new GameObject().AddComponent<ImportObj>();
        GameObject go = new GameObject();
        yield return null;
        importObj.AttachComponents(go);
        Assert.NotNull(go.GetComponent<Model>());
    }

    [UnityTest]
    public IEnumerator _Check_Imported_Object_Has_Rigibody_Component()
    {
        var importObj = new GameObject().AddComponent<ImportObj>();
        GameObject go = new GameObject();
        yield return null;
        importObj.AttachComponents(go);
        Assert.NotNull(go.GetComponent<Rigidbody>());
    }

    [UnityTest]
    public IEnumerator _Check_Imported_Object_Collision_Detection_ON()
    {
        var importObj = new GameObject().AddComponent<ImportObj>();
        GameObject go = new GameObject();
        yield return null;
        importObj.AttachComponents(go);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        Assert.Equals(true,rb.detectCollisions);
    }

}
