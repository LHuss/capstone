using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManipulation : MonoBehaviour { 
	
	private static float COLLISION_PRECISION = 0.05F;	

	void OnCollisionEnter(Collision collision){
		MeshFilter objectMesh = GetComponent<MeshFilter>();
		Vector3[] vertices = objectMesh.mesh.vertices;
		Vector3[] normals = objectMesh.mesh.normals;

		ContactPoint[] contactPoints = collision.contacts;
		for (int i = 0; i < vertices.Length; i++){
			Vector3 globalPoint = transform.TransformPoint(vertices[i]);
			foreach (ContactPoint contact in contactPoints){
				if (SameGlobalPoint(contact.point, globalPoint)){
					Vector3 nerfedNormal = new Vector3(
						contact.normal.x * (float) 0.001,
						contact.normal.y * (float) 0.001,
						contact.normal.z * (float) 0.001);
					vertices[i] = transform.InverseTransformPoint(globalPoint + nerfedNormal);
				}
			}
		}
		objectMesh.mesh.vertices = vertices;
		objectMesh.mesh.RecalculateNormals();
		objectMesh.mesh.RecalculateBounds();
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = objectMesh.mesh;
	}

	private static bool SameGlobalPoint(Vector3 v1, Vector3 v2)
	{
		return !(Mathf.Abs(v1.x - v2.x) > COLLISION_PRECISION ||
			     Mathf.Abs(v1.y - v2.y) > COLLISION_PRECISION ||
			     Mathf.Abs(v1.z - v2.z) > COLLISION_PRECISION);
	}
}
