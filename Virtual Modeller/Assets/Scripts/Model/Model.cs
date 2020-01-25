using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour{
	private static MeshFilter _meshFilter;
	public float scale;
	public List<Vector3> vertices;
	public List<Vector3> normals;
    public List<int> triangles;

    /*
	*	Assign Mesh filter to class variable to reduce mem-alloc each time
	*	a collision occurs
	*/
	public void Awake(){
		_meshFilter = GetComponent<MeshFilter>();
        vertices = new List<Vector3>(_meshFilter.mesh.vertices);
        normals = new List<Vector3>(_meshFilter.mesh.normals);
        triangles = new List<int>(_meshFilter.mesh.triangles);
        UpdateMesh();
		UpdateCollider();
		Debug.Log("Initialized Model");
	}

    public void Subdivide()
    {
        Debug.Log("Pre-subdivision vertex count: " + vertices.Count);
        // Dictionary containing newly generated vertices, used to check if vertex already exists
        Dictionary<uint,int> newVertices = new Dictionary<uint,int>();

        List<int> newTriangles = new List<int>();

        for (int i = 0; i < triangles.Count; i+=3)
        {
            int i1 = triangles[i];
            int i2 = triangles[i+1];
            int i3 = triangles[i+2];

            int a = _GetNewVertex(i1, i2, newVertices);
            int b = _GetNewVertex(i2, i3, newVertices);
            int c = _GetNewVertex(i3, i1, newVertices);

            newTriangles.Add(i1);   newTriangles.Add(a);   newTriangles.Add(c); // top triangle
            newTriangles.Add(i2);   newTriangles.Add(b);   newTriangles.Add(a); // left triangle
            newTriangles.Add(i3);   newTriangles.Add(c);   newTriangles.Add(b); // right triangle
            newTriangles.Add(a);   newTriangles.Add(b);   newTriangles.Add(c);  // center triangle
        }
        triangles = newTriangles;
        UpdateMesh();
        UpdateCollider();
        Debug.Log("Post-subdivision vertex count: " + vertices.Count);

        // Based on Bunny83's answer https://bit.ly/33khaNj
    }

	// reassign computed vertices to mesh vertices (update mesh for rendering)
	public void UpdateMesh(){
        _meshFilter.mesh.vertices = vertices.ToArray();
        _meshFilter.mesh.normals = normals.ToArray();
        _meshFilter.mesh.triangles = triangles.ToArray();
	}
	
	// reassign computed mesh to mesh collider (update mesh for collision)
	public void UpdateCollider(){
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = _meshFilter.mesh;
	}

    // return an object array representation of the current state
    public object[] GetCurrentStateRepresentation(){
        object[] currentState = new object[4];
        currentState[0] = this.scale;
        currentState[1] = new List<Vector3>(this.vertices);
        currentState[2] = new List<Vector3>(this.normals);
        currentState[3] = new List<int>(this.triangles);
        return currentState;
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
        int newIndex = vertices.Count;
        newVectices.Add(t1, newIndex);
        // Add vertex between i1 & i2, its normal is i1's + i2's norm
        vertices.Add((vertices[i1] + vertices[i2]) * 0.5F);
        normals.Add((normals[i1] + normals[i2]).normalized);
        return newIndex;
    }
}