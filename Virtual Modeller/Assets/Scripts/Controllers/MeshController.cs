using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : Singleton<MeshController> {
	private DeformationType _deformationType;
	private static float _collisionAccuracy;
	private static float _deformationForce;
	private static int STATE_SAVE_RATE = 120; // Around 2 seconds at 60fps
	private static int MAXIMUM_STATES_COUNT = 50;
	private static Model _model;
	private static LinkedList<object[]> _states;
	private static LinkedListNode<object[]> _currentState;
	private static int _stateTimer;
	private static bool isNewState = false;


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
		_stateTimer = STATE_SAVE_RATE;
		AttachMesh(this.gameObject);
		_model.Subdivide();
		_model.UpdateMesh();
		_model.UpdateCollider();
		_states = new LinkedList<object[]>();
		_states.AddLast(_model.GetCurrentStateRepresentation());
		_currentState = _states.First;
	}

	void Update(){
        if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			if(Input.GetKeyDown(KeyCode.Z)){
				Undo();
			}
			if(Input.GetKeyDown(KeyCode.Y)){ 
				Redo();
			}
		}
		if(_stateTimer > 0){
			_stateTimer--;
		}
		else if (_stateTimer == 0 && isNewState){
			// clear the future states to prevent illegal redo
			while(_states.Last != _currentState){
				_states.RemoveLast();
			}
			_states.AddLast(_model.GetCurrentStateRepresentation());
			_currentState = _states.Last;
			Debug.Log("Count of saved model states: " + _states.Count);
			if(_states.Count > MAXIMUM_STATES_COUNT){
				_states.RemoveFirst();
			}
			_stateTimer = STATE_SAVE_RATE;
			isNewState = false;
		}
	}

	/*
	*	Called when collision occurs between this gameObject and another
	*	Only occurs when isKinematic is enabled for a gameObject's rigidBody
	*/
	public void OnCollisionEnter(Collision collision){
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
		isNewState = true;
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
		if(_currentState.Previous != null && _currentState.Previous != _states.Last){
			Debug.Log("Undo performed.");
			_currentState = _currentState.Previous;
			AssignStateToModel(_currentState.Value);
		}
	}

	// redo the last undo
	public void Redo(){
		if(_currentState.Next != null && _currentState.Next != _states.First){
			Debug.Log("Redo performed.");
			_currentState = _currentState.Next;
			AssignStateToModel(_currentState.Value);
		}
	}

	private void AssignStateToModel(object[] state){
		_model.scale = (float) state[0];
		_model.vertices = (List<Vector3>) state[1];
		_model.normals = (List<Vector3>) state[2];
		_model.triangles = (List<int>) state[3];
		_model.UpdateMesh();
		_model.UpdateCollider();
	}

	public static bool SameGlobalPoint(Vector3 v1, Vector3 v2){
		return !(Mathf.Abs(v1.x - v2.x) > _collisionAccuracy ||
			     Mathf.Abs(v1.y - v2.y) > _collisionAccuracy ||
			     Mathf.Abs(v1.z - v2.z) > _collisionAccuracy);
	}
}
