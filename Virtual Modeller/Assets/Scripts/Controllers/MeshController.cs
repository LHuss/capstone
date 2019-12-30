using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : Singleton<MeshController> {
	private static Model _model;
	private DeformationType _deformationType;
	private static int _collisionAccuracy;
	private static float _deformationForce;


	public Model Model {
		get{ return _model; }
		set{ _model = value; }
	}
	public DeformationType DeformationType {
		get{ return _deformationType; }
		set{ _deformationType = value; }
	}
	public int CollisionAccuracy {
		get{ return _collisionAccuracy; }
		set{ _collisionAccuracy = value; }
	}
	public float CollisionForce {
		get{ return _deformationForce; }
		set{ _deformationForce = value; }
	}

	/*
	*	Used to assign new mesh to gameObject
	*	Ex: Import new mesh -> assign _model as that mesh
	*/
	public void AttachMesh(GameObject gameObject){
		_model = gameObject.AddComponent(typeof(Model)) as Model;
	}

	void Awake(){
		Debug.Log("Initialize MeshController");
		_deformationType = DeformationType.PUSH;
		_deformationForce = 0.01F;
		_collisionAccuracy = 4;
		AttachMesh(this.gameObject);
		_model.Subdivide();
		_model.UpdateVerticesDict(_collisionAccuracy);
	}

	/*
	*	Called when collision occurs between this gameObject and another
	*	Only occurs when isKinematic is enabled for a gameObject's rigidBody
	*/
	void OnCollisionEnter(Collision collision){
		// Check each of model's vertex (Global position) against
		// collision point (Global position), deform mesh if they are the same
		// TODO: mesh deformation optimization (checking of mesh vertex against contact point)
		ContactPoint[] contactPoints = collision.contacts;
		foreach (ContactPoint contact in contactPoints){
			string contactKey = (
				contact.point.x.ToString().Substring(0, _collisionAccuracy) +
				contact.point.y.ToString().Substring(0, _collisionAccuracy) +
				contact.point.z.ToString().Substring(0, _collisionAccuracy)
			);
			if (_model.VerticesDict.ContainsKey(contactKey)){
				// Modify all points under the same key
				for(int i = 0; i < _model.VerticesDict[contactKey].Count; i++){
					int vertexIndex = _model.VerticesDict[contactKey][i];
					// These points need to be more accurate because they are the actual mesh vertices
					_model.Vertices[vertexIndex] = _Deform(
						transform.TransformPoint(_model.Vertices[vertexIndex]),
						contact.normal);
				}
				// Assign new key to vertices list after deformation
				Vector3 newPoint = _Deform(
					transform.TransformPoint(contact.point),
					contact.normal);
				string newKey = (
					newPoint.x.ToString().Substring(0, _collisionAccuracy) +
					newPoint.y.ToString().Substring(0, _collisionAccuracy) +
					newPoint.z.ToString().Substring(0, _collisionAccuracy)
				);
				_model.VerticesDict[newKey] = _model.VerticesDict[contactKey];
				_model.VerticesDict.Remove(contactKey);
			}
		}
		_model.UpdateMesh();
		_model.UpdateCollider();
	}
	
	private Vector3 _Deform(Vector3 point, Vector3 normal){
		// transform global position to mesh's local position
		if(_deformationType == DeformationType.PUSH)
			return transform.InverseTransformPoint(point + _GetCollisionNormal(normal));
		else
			return transform.InverseTransformPoint(point - _GetCollisionNormal(normal));
	}

	// returns deformation intensity depending on DEFORMATION_FORCE
	private Vector3 _GetCollisionNormal(Vector3 collisionNormal){
		return collisionNormal * _deformationForce;
	}
}
