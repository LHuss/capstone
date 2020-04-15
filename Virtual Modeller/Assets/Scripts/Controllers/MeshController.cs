using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : Singleton<MeshController> {
	private DeformationType _deformationType;
	private static float _collisionAccuracy;
	private static int _collisionDecimals;
	private static float _deformationForce;
	private static int STATE_SAVE_RATE = 120; // Around 2 seconds at 60fps
	private static int MAXIMUM_STATES_COUNT = 50;
	private static Model _model;
	private static LinkedList<object[]> _states;
	private static LinkedListNode<object[]> _currentState;
	private static int _stateTimer;
	private static bool isNewState = false;
	private static bool modelWasUpdated;


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
		gameObject.AddComponent<Model>();
		_model = gameObject.GetComponent<Model>();
	}

	void Start(){
		Debug.Log("Initialize MeshController");
		_deformationType = DeformationType.PUSH;
		_deformationForce = 0.01F;
		_collisionAccuracy = 0.04F;
		_stateTimer = STATE_SAVE_RATE;
		_collisionDecimals = 2;

		AttachMesh(this.gameObject);
		_model.Start();
		if(_model.vertices.Count < 2000){
			_model.Subdivide();
		}
		_model.UpdateMesh();
		_model.UpdateCollider();
		//_model.ResetVerticesDict(_collisionDecimals);
		_model.ResetIndexNeighborDict();
		_states = new LinkedList<object[]>();
		_states.AddLast(_model.GetCurrentStateRepresentation());
		_currentState = _states.First;
	}

	public void DestroyModel() {
		if(!!_model && !!_model.gameObject) {
			Debug.Log("Destroying Model");
			Destroy(_model.gameObject);
		}
	}

	void Update(){
		if(!!_model) {
			if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
				if(Input.GetKeyDown(KeyCode.Z)){
					Undo();
				}
				if(Input.GetKeyDown(KeyCode.Y)){ 
					Redo();
				}
			}
			if(Input.GetKeyDown(KeyCode.L)) {
				_model.ApplyGlobalLaplacianFilter();
			}
			if(_stateTimer > 0){
				_stateTimer--;
			}
			else if (_stateTimer == 0 && isNewState){
				_states.AddLast(_model.GetCurrentStateRepresentation());
				_currentState = _states.Last;
				Debug.Log("Count of saved model states: " + _states.Count);
				if(_states.Count > MAXIMUM_STATES_COUNT){
					_states.RemoveFirst();
				}
				_stateTimer = STATE_SAVE_RATE;
				isNewState = false;
			}
			if (modelWasUpdated && _model.TrySmoothing()) {
				modelWasUpdated = false;
			}
			
			// scale down
			Vector3 modelScale = _model.gameObject.transform.localScale;
			Vector3 scalingVector = new Vector3(1f, 1f, 1f) * 0.001f;
			if (Input.GetKey(",")) {
				modelScale -= scalingVector;
			}
			// scale up
			if (Input.GetKey("."))
			{
				modelScale += scalingVector;
			}
			_model.gameObject.transform.localScale = modelScale;	
		}
	}

	/*
	*	Called when collision occurs between this gameObject and another
	*	Only occurs when isKinematic is enabled for a gameObject's rigidBody
	*/
	public void OnCollisionEnter(Collision collision){
		HandleAndUpdate(collision);
	}

	public void OnCollisionStay(Collision collision) {
		HandleAndUpdate(collision);
	}

	public void HandleAndUpdate(Collision collision) {
		HandleCollision(collision);

		foreach (Model m in GetComponents<Model>()) {
			m.UpdateMesh();
			m.UpdateCollider();
		}
		// clear the future states to prevent illegal redo
		while(_states.Last != _currentState){
			_states.RemoveLast();
		}
		isNewState = true;
	}

	public void HandleCollision(Collision collision) {
		// Check each of model's vertex (Global position) against
		// collision point (Global position), deform mesh if they are about the same
		// TODO: mesh deformation optimization (checking of mesh vertex against contact point)
		ContactPoint[] contactPoints = collision.contacts;
		for (int i = 0; i < _model.vertices.Count; i++){
			Vector3 globalPoint = transform.TransformPoint(_model.vertices[i]);
			for (int j = 0; j < contactPoints.Length; j++){
				if (SameGlobalPoint(contactPoints[j].point, globalPoint)){
					Vector3 newVertex = transform.InverseTransformPoint(
						Deform(globalPoint, contactPoints[j].normal)
					);
					_model.UpdateVertex(i, newVertex);
					modelWasUpdated = true;
				}
			}
		}
	}

	private void HandleCollisionDict(Collision collision) {
		ContactPoint[] contactPoints = collision.contacts;

		foreach (ContactPoint contact in contactPoints){
			Vector3 contactPoint = contact.point;
			string contactKey = VertexHelper.HashVertex(contactPoint, _collisionDecimals);


			if (_model.verticesDict.ContainsKey(contactKey)){
				Vector3 newWorldPosition = Deform(
					contactPoint, 
					contact.normal
				);

				Vector3 newLocalPosition = transform.InverseTransformPoint(newWorldPosition);

				// Modify all vertices under the same key
				for(int i = 0; i < _model.verticesDict[contactKey].Count; i++){
					int vertexIndex = _model.verticesDict[contactKey][i];
					Vector3 vertex = _model.vertices[vertexIndex];
					vertex.x = newLocalPosition.x;
					vertex.y = newLocalPosition.y;
					vertex.z = newLocalPosition.z;
					_model.vertices[vertexIndex] = vertex;
				}

				string newKey = VertexHelper.HashVertex(newWorldPosition, _collisionDecimals);

				_model.TransferKey(contactKey, newKey);	
			}
		}
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
		return collisionNormal * _deformationForce * ToolController.Instance.Hardness;
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
