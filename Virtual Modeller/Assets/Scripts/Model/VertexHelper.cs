using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VertexHelper {
	public static string HashVertex(Vector3 vertex, int accuracy) {
        string format = "0.";
        for (int i = 0; i < accuracy; i++) {
            format += "0";
        }

		string key = (
			vertex.x.ToString(format) +
			vertex.y.ToString(format) +
			vertex.z.ToString(format)
		);

		return key;
	}

	public static List<int> FindNeighboringVertexIndeces(List<Vector3> vertices, List<int> triangles, Vector3 vertex) {
		List<int> neighbors = new List<int>();
		
		int triangleCount = 0;

		for(int i=0; i < vertices.Count; i++) {

			// first, find matching vertex
			if (IsMatch(vertex, vertices[i])) {
				int neighbor1 = 0;
				int neighbor2 = 0;
				bool isContained = false;

				// Check each triangle to see if vertex index is index in the triangle
				// Note - triangle is a list of vertex indices, which is why I check i (the index of the vertex) and not the vertex itself here
				for (int j = 0; j<triangles.Count; j = j+3) {
					if (i == triangles[j]) {
						neighbor1 = triangles[j+1];
						neighbor2 = triangles[j+2];
						isContained = true;
					} else if (i == triangles[j+1]) {
						neighbor1 = triangles[j];
						neighbor2 = triangles[j+2];
						isContained = true;
					} else if (i == triangles[j+2]) {
						neighbor1 = triangles[j];
						neighbor2 = triangles[j+1];
						isContained = true;
					}

					// If there's a hit, increment number of triangles the vertex is a part of (for debugging)
					// Then, add the adjacent indices to the list of neighbors if they're not already in the list
					if (isContained) {
						triangleCount++;

						if (!neighbors.Contains(neighbor1)) {
							neighbors.Add(neighbor1);
						}
						if (!neighbors.Contains(neighbor2)) {
							neighbors.Add(neighbor2);
						}
					}
				}
			}
		}

		// Debug.Log(string.Format("# of triangles Vertex({0}) is a part of : {1}", HashVertex(vertex, 2), triangleCount));

		return neighbors;
	}
	public static bool IsMatch(Vector3 v1, Vector3 v2) {
		return (
			Mathf.Approximately(v1.x, v2.x)
			&& Mathf.Approximately(v1.y, v2.y)
			&& Mathf.Approximately(v1.z, v2.z)
		);
	}
}