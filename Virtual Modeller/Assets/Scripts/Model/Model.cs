using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour{
	private static MeshFilter _meshFilter;
	private float scale;
	public List<Vector3> _vertices;
	private List<Vector3> _normals;
    private List<int> _triangles;
    private Dictionary<string,List<int>> _verticesDict;

	public float Scale { get; set; }
    public List<Vector3> Vertices { get { return _vertices; } private set{} }
	public List<Vector3> Normals { get {return _normals; } private set{} }
	public List<int> Triangles { get {return _triangles; } private set{} }
	public Dictionary<string,List<int>> VerticesDict {
        get{ return _verticesDict; }
        set{ _verticesDict = value; }
    }

    /*
	*	Assign Mesh filter to class variable to reduce mem-alloc each time
	*	a collision occurs
	*/
	public void Awake(){
		_meshFilter = GetComponent<MeshFilter>();
        _vertices = new List<Vector3>(_meshFilter.mesh.vertices);
        _normals = new List<Vector3>(_meshFilter.mesh.normals);
        _triangles = new List<int>(_meshFilter.mesh.triangles);
        UpdateMesh();
		UpdateCollider();
		Debug.Log("Initialized Model");
	}

    public void Subdivide()
    {
        Debug.Log("Pre-subdivision vertex count: " + _vertices.Count);
        // Dictionary containing newly generated vertices, used to check if vertex already exists
        Dictionary<uint,int> newVertices = new Dictionary<uint,int>();

        List<int> newTriangles = new List<int>();

        for (int i = 0; i < _triangles.Count; i+=3)
        {
            int i1 = _triangles[i];
            int i2 = _triangles[i+1];
            int i3 = _triangles[i+2];

            int a = _GetNewVertex(i1, i2, newVertices);
            int b = _GetNewVertex(i2, i3, newVertices);
            int c = _GetNewVertex(i3, i1, newVertices);

            newTriangles.Add(i1);   newTriangles.Add(a);   newTriangles.Add(c); // top triangle
            newTriangles.Add(i2);   newTriangles.Add(b);   newTriangles.Add(a); // left triangle
            newTriangles.Add(i3);   newTriangles.Add(c);   newTriangles.Add(b); // right triangle
            newTriangles.Add(a);   newTriangles.Add(b);   newTriangles.Add(c);  // center triangle
        }
        _triangles = newTriangles;
        UpdateMesh();
        UpdateCollider();
        Debug.Log("Post-subdivision vertex count: " + _vertices.Count);

        // Based on Bunny83's answer https://bit.ly/33khaNj
    }

	// reassign computed vertices to mesh vertices (update mesh for rendering)
	public void UpdateMesh(){
        _meshFilter.mesh.vertices = _vertices.ToArray();
        _meshFilter.mesh.normals = _normals.ToArray();
        _meshFilter.mesh.triangles = _triangles.ToArray();
	}
	
	// reassign computed mesh to mesh collider (update mesh for collision)
	public void UpdateCollider(){
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = _meshFilter.mesh;
	}    

	public void UpdateVerticesDict(int accuracy){
        // Global Vector3 point will point to corresponding mesh vertex's index
        _verticesDict = new Dictionary<string, List<int>>();
		for(int i = 0; i < _vertices.Count; i++){
			Vector3 globalV = transform.TransformPoint(_vertices[i]);
			string key = (
                globalV.x.ToString().Substring(0, accuracy) +
                globalV.y.ToString().Substring(0, accuracy) +
                globalV.z.ToString().Substring(0, accuracy)
            );
			if(!_verticesDict.ContainsKey(key)){
				_verticesDict.Add(key, new List<int>(){i});
			}
            else{
                _verticesDict[key].Add(i);
            }
		}
	}

    private int _GetNewVertex(int i1, int i2, Dictionary<uint, int> newVectices)
    {
        // 16bit int value, shift i1 & i2
        uint t1 = ((uint)i1 << 16) | (uint)i2;
        uint t2 = ((uint)i2 << 16) | (uint)i1;
        if (newVectices.ContainsKey(t2))
            return newVectices[t2];
        if (newVectices.ContainsKey(t1))
            return newVectices[t1];

        // generate vertex:
        int newIndex = _vertices.Count;
        newVectices.Add(t1, newIndex);
        // Add vertex between i1 & i2, its normal is i1's + i2's norm
        _vertices.Add((_vertices[i1] + _vertices[i2]) * 0.5F);
        _normals.Add((_normals[i1] + _normals[i2]).normalized);
        return newIndex;
    }
}
