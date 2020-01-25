using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : Singleton<MeshController> {
	private static Model _model;
	private DeformationType _deformationType;
	private static float _collisionAccuracy;
	private static float _deformationForce;
	private static Stack<object[]> _previousStates;
	private static Stack<object[]> _nextStates;


	public Model Model {
		get{ return _model; }
		set{ _model = value; }
	}
	public DeformationType DeformationType {
		get{ return _deformationType; }
		set{ _deformationType = value; }
	}
	public float CollisionAccuracy {
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
		_collisionAccuracy = 0.04F;
		_previousStates = new Stack<object[]>();
		_nextStates = new Stack<object[]>();
		AttachMesh(this.gameObject);
		_model.Subdivide();
	}

	void Update(){
        if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			if(Input.GetKey(KeyCode.Z)){
				Undo();
			}
			if(Input.GetKey(KeyCode.Y)){ 
				Redo();
			}
		}
	}

	/*
	*	Called when collision occurs between this gameObject and another
	*	Only occurs when isKinematic is enabled for a gameObject's rigidBody
	*/
	public void OnCollisionEnter(Collision collision){
		// Save previous state to allow undo, clear the nextStates to prevent illegal redo
		_previousStates.Push(_model.GetCurrentStateRepresentation());
		_nextStates.Clear();
		// Check each of model's vertex (Global position) against
		// collision point (Global position), deform mesh if they are about the same
		// TODO: mesh deformation optimization (checking of mesh vertex against contact point)
		ContactPoint[] contactPoints = collision.contacts;
		for (int i = 0; i < _model.vertices.Count; i++){
			Vector3 globalPoint = transform.TransformPoint(_model.vertices[i]);
			for (int j = 0; j < contactPoints.Length; j++){
				if (SameGlobalPoint(contactPoints[j].point, globalPoint)){
					_model.vertices[i] = transform.InverseTransformPoint(
						Deform(globalPoint, contactPoints[j].normal));
				}
			}
		}
		_model.UpdateMesh();
		_model.UpdateCollider();
	}
	
	public Vector3 Deform(Vector3 point, Vector3 normal){
		// transform global position to mesh's local position
		if(_deformationType == DeformationType.PUSH)
			return point + _GetCollisionNormal(normal);
		else
			return point - _GetCollisionNormal(normal);
	}

	// returns deformation intensity depending on DEFORMATION_FORCE
	private Vector3 _GetCollisionNormal(Vector3 collisionNormal){
		return collisionNormal * _deformationForce;
	}

	// undo the last modification performed on the model
	public void Undo(){
		if(_previousStates.Count != 0){
			Debug.Log("Undo performed.");
			_nextStates.Push(_model.GetCurrentStateRepresentation());
			AssignStateToModel(_previousStates.Pop());
			_model.UpdateMesh();
			_model.UpdateCollider();
		}
	}

	// redo the last undo
	public void Redo(){
		if(_nextStates.Count != 0){
			Debug.Log("Redo performed.");
			_previousStates.Push(_model.GetCurrentStateRepresentation());
			AssignStateToModel(_nextStates.Pop());
			_model.UpdateMesh();
			_model.UpdateCollider();
		}
	}

	private void AssignStateToModel(object[] state){
		_model.scale = (float) state[0];
		_model.vertices = (List<Vector3>) state[1];
		_model.normals = (List<Vector3>) state[2];
		_model.triangles = (List<int>) state[3];
	}

	public static bool SameGlobalPoint(Vector3 v1, Vector3 v2){
		return !(Mathf.Abs(v1.x - v2.x) > _collisionAccuracy ||
			     Mathf.Abs(v1.y - v2.y) > _collisionAccuracy ||
			     Mathf.Abs(v1.z - v2.z) > _collisionAccuracy);
	}
}
