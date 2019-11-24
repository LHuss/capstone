using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : Singleton<MeshController> {
	private static Model _model;
	private DeformationType _deformationType;
	private static float _collisionPrecision;
	private static float _deformationForce;


	public Model Model {
		get{ return _model; }
		set{ _model = value; }
	}
	public DeformationType DeformationType {
		get{ return _deformationType; }
		set{ _deformationType = value; }
	}
	public float CollisionPrecision {
		get{ return _collisionPrecision; }
		set{ _collisionPrecision = value; }
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
		_deformationForce = 0.001F;
		_collisionPrecision = 0.04F;
		AttachMesh(this.gameObject);
		// _model.Subdivide();
	}

	/*
	*	Called when collision occurs between this gameObject and another
	*	Only occurs when isKinematic is enabled for a gameObject's rigidBody
	*/
	void OnCollisionEnter(Collision collision){
		// Check each of model's vertex (Global position) against
		// collision point (Global position), deform mesh if they are about the same
		// TODO: mesh deformation optimization (checking of mesh vertex against contact point)
		ContactPoint[] contactPoints = collision.contacts;
		for (int i = 0; i < _model.Vertices.Count; i++){
			Vector3 globalPoint = transform.TransformPoint(_model.Vertices[i]);
			foreach (ContactPoint contact in contactPoints){
				if (_SameGlobalPoint(contact.point, globalPoint)){
					_model.Vertices[i] = _Deform(globalPoint, contact.normal);
				}
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

	private static bool _SameGlobalPoint(Vector3 v1, Vector3 v2){
		return !(Mathf.Abs(v1.x - v2.x) > _collisionPrecision ||
			     Mathf.Abs(v1.y - v2.y) > _collisionPrecision ||
			     Mathf.Abs(v1.z - v2.z) > _collisionPrecision);
	}
}
