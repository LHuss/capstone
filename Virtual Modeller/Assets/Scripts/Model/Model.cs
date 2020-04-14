using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour{
    public float scale;
	public List<Vector3> vertices;
	public List<Vector3> normals;
    public List<int> triangles;
    public Dictionary<string, List<int>> verticesDict;
    public Dictionary<int, List<int>> indexNeighborDict;
    public HashSet<int> editedIndices;
    public long lastEdited;

    /*
	*	Assign Mesh filter to class variable to reduce mem-alloc each time
	*	a collision occurs
	*/
	public void Start(){
        vertices = new List<Vector3>(GetComponent<MeshFilter>().sharedMesh.vertices);
        normals = new List<Vector3>(GetComponent<MeshFilter>().sharedMesh.normals);
        triangles = new List<int>(GetComponent<MeshFilter>().sharedMesh.triangles);
        editedIndices = new HashSet<int>();
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
        Debug.Log("Post-subdivision vertex count: " + vertices.Count);

        // Based on Bunny83's answer https://bit.ly/33khaNj
    }

	// reassign computed vertices to mesh vertices (update mesh for rendering)
	public void UpdateMesh(){
        GetComponent<MeshFilter>().mesh.vertices = vertices.ToArray();
        GetComponent<MeshFilter>().mesh.normals = normals.ToArray();
        GetComponent<MeshFilter>().mesh.triangles = triangles.ToArray();        
	}
	
	// reassign computed mesh to mesh collider (update mesh for collision)
	public void UpdateCollider(){
		GetComponent<MeshCollider>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
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

    public void ResetVerticesDict(int accuracy) {
        Debug.Log("Recomputing Vertices Dictionary");
        verticesDict = new Dictionary<string, List<int>>();
        for (int i=0; i < vertices.Count; i++) {
            Vector3 worldPos = transform.TransformPoint(vertices[i]);

            string key = VertexHelper.HashVertex(worldPos, accuracy);

            if (!verticesDict.ContainsKey(key)) {
                verticesDict.Add(key, new List<int>(){i});
			}
            else {
                verticesDict[key].Add(i);
            }
        }
    }

    public void ResetIndexNeighborDict() {
        Debug.Log("Recomputing Index-Neighbor Dictionary");
        indexNeighborDict = new Dictionary<int, List<int>>();
        indexNeighborDict = ModelHelper.CreateIndexNeighborDictionary(this);
    }

    public void ApplyGlobalLaplacianFilter() {
        vertices = ModelHelper.ComputeGlobalLaplacianFilter(this);
		UpdateMesh();
		UpdateCollider();
    }

    public void ApplyLocalLaplacianFilter() {
        ApplyLocalLaplacianFilterTimes(1);
    }

    public void ApplyLocalLaplacianFilterTimes(int iterations) {
        this.vertices = ModelHelper.ComputeLaplacianFilterTimes(this.vertices, this.indexNeighborDict, iterations, this.editedIndices);
		UpdateMesh();
		UpdateCollider();
        this.editedIndices.Clear();
    }

	public void TransferKey(string oldKey, string newKey) {
        List<int> indecesToMove = verticesDict[oldKey];
        
		if (verticesDict.ContainsKey(newKey)) {
            for (int i = 0; i < indecesToMove.Count; i++) {
                verticesDict[newKey].Add(verticesDict[oldKey][i]);
            }
		} else {
            verticesDict.Add(newKey, indecesToMove);
        }
        
        verticesDict.Remove(oldKey);
	}

    public void UpdateVertex(int index, Vector3 vertex) {
        this.vertices[index] = vertex;
		this.editedIndices.Add(index);
        this.lastEdited = DateTime.Now.Ticks;
    }

    public bool TrySmoothing() {
        long now = DateTime.Now.Ticks;
        long elapsedTicks = now - this.lastEdited;
        TimeSpan elapsed = new TimeSpan(elapsedTicks);

        if(elapsed.TotalSeconds >= 3) {
            this.ApplyLocalLaplacianFilter();
            this.lastEdited = now;
            return true;
        }
        return false;
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