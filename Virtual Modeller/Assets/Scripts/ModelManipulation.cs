using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManipulation : MonoBehaviour { 
	
	private static float COLLISION_PRECISION = 0.05F;
	private static float DEFORMATION_FORCE = 1F;
 	private MeshFilter objectMesh;
	
	/*
	*	Assign Mesh filter to class variable to reduce mem-alloc each time
	*	a collision occurs
	*/
	void Awake(){
		objectMesh = GetComponent<MeshFilter>();
	}

	/*
	*	Called when collision occurs between this gameObject and another
	*	Only occurs when isKinematic is enabled for a gameObject's rigidBody
	*/
	void OnCollisionEnter(Collision collision){
		Vector3[] vertices = objectMesh.mesh.vertices;
		Vector3[] normals = objectMesh.mesh.normals;
		ContactPoint[] contactPoints = collision.contacts;

		// Check each of model's vertex (Global position) against
		// collision point (Global position), deform mesh if they are about the same
		// TODO: mesh deformation optimization (checking of mesh vertex against contact point)
		for (int i = 0; i < vertices.Length; i++){
			Vector3 globalPoint = transform.TransformPoint(vertices[i]);
			foreach (ContactPoint contact in contactPoints){
				if (_sameGlobalPoint(contact.point, globalPoint)){
					vertices[i] = _pushDeformation(globalPoint, contact.normal);
				}
			}
		}
		_updateMesh(vertices);
	}

	private Vector3 _pushDeformation(Vector3 point, Vector3 normal){
		// transform global position to mesh's local position
		return transform.InverseTransformPoint(point + _getCollisionNormal(normal));
	}

	private Vector3 _pullDeformation(Vector3 point, Vector3 normal){
		// transform global position to mesh's local position
		return transform.InverseTransformPoint(point - _getCollisionNormal(normal));
	}

	// returns deformation intensity depending on DEFORMATION_FORCE
	private Vector3 _getCollisionNormal(Vector3 collisionNormal){
		return collisionNormal * DEFORMATION_FORCE * 0.001F;
	}

	// reassign computed vertices to mesh vertices (update mesh for rendering)
	private void _updateMesh(Vector3[] localVertices){
		objectMesh.mesh.vertices = localVertices;
		objectMesh.mesh.RecalculateNormals();
		objectMesh.mesh.RecalculateBounds();
		_updateCollider();
	}

	// reassign computed mesh to mesh collider (update mesh for collision)
	private void _updateCollider(){
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = objectMesh.mesh;
	}

	private static bool _sameGlobalPoint(Vector3 v1, Vector3 v2)
	{
		return !(Mathf.Abs(v1.x - v2.x) > COLLISION_PRECISION ||
			     Mathf.Abs(v1.y - v2.y) > COLLISION_PRECISION ||
			     Mathf.Abs(v1.z - v2.z) > COLLISION_PRECISION);
	}
}
