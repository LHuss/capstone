using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour{
	private static MeshFilter _meshFilter;
	private float scale;
	private Vector3[] _vertices;
	private Vector3[] _normals;

	public float Scale { get; set; }
    public Vector3[] Vertices { get { return _vertices; } private set{} }
	public Vector3[] Normals { get {return _normals; } private set{} }

    
    /*
	*	Assign Mesh filter to class variable to reduce mem-alloc each time
	*	a collision occurs
	*/
	public void Awake(){
		Debug.Log("Initialize Model");
		_meshFilter = GetComponent<MeshFilter>();
        _vertices = _meshFilter.mesh.vertices;
        _normals = _meshFilter.mesh.normals;
        UpdateMesh();
		UpdateCollider();
	}

	// reassign computed vertices to mesh vertices (update mesh for rendering)
	public void UpdateMesh(){
		_meshFilter.mesh.vertices = _vertices;
	}
	
	// reassign computed mesh to mesh collider (update mesh for collision)
	public void UpdateCollider(){
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = _meshFilter.mesh;
	}
}
