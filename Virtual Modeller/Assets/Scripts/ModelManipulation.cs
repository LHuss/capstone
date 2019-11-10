using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManipulation : MonoBehaviour { 
	
	void OnCollisionEnter(Collision collision){
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;

		ContactPoint[] contactPoints = collision.contacts;
		for (int i = 0; i < vertices.Length; i++){
			Vector3 globalPoint = transform.TransformPoint(vertices[i]);
			foreach (ContactPoint contact in contactPoints){
				if (AlmostEqual(contact.point, globalPoint, (float) 0.1)){
					Vector3 nerfedNormal = new Vector3(
						contact.normal.x * (float) 0.001,
						contact.normal.y * (float) 0.001,
						contact.normal.z * (float) 0.001);
					vertices[i] = transform.InverseTransformPoint(globalPoint + nerfedNormal);
				}
			}
		}
		mesh.vertices = vertices;
	}	
	
	private static bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
	{
		bool equal = true;
		if (Mathf.Abs (v1.x - v2.x) > precision) equal = false;
		if (Mathf.Abs (v1.y - v2.y) > precision) equal = false;
		if (Mathf.Abs (v1.z - v2.z) > precision) equal = false;
		return equal;
	}
}
