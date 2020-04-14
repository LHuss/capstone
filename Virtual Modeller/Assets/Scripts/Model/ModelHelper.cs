using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModelHelper {
	public static Dictionary<int, List<int>> CreateIndexNeighborDictionary(Model model) {
		Dictionary<int, List<int>> indexNeighborDict = new Dictionary<int, List<int>>();

		List<Vector3> vertices = model.vertices;
		List<int> triangles = model.triangles;
		for (int i = 0; i < vertices.Count; i++) {
			Vector3 vertex = vertices[i];
			List<int> neighboringVertexIndeces = VertexHelper.FindNeighboringVertexIndeces(vertices, triangles, vertex);

			indexNeighborDict.Add(i, neighboringVertexIndeces);
		}

		return indexNeighborDict;
	}

	public static List<Vector3> ComputeGlobalLaplacianFilter(Model model) {
		return ComputeGlobalLaplacianFilterTimes(model, 1);
	}

	public static List<Vector3> ComputeGlobalLaplacianFilterTimes(Model model, int iterations) {
		List<Vector3> vertices = model.vertices;
		Dictionary<int, List<int>> indexNeighborDict = model.indexNeighborDict;

		return ComputeLaplacianFilterTimes(vertices, indexNeighborDict, iterations);
	}

	public static List<Vector3> ComputeLaplacianFilterTimes(List<Vector3> vertices, Dictionary<int, List<int>> indexNeighborDict, int iterations, HashSet<int> indicesToUpdate = null) {
        Debug.Log(string.Format("Applying Laplacian Filter {0} time{1}", iterations, iterations > 1 ? "s" : ""));
		if (iterations == 0) return vertices;

		List<Vector3> newVertices = new List<Vector3>();
		newVertices = ComputeLaplacianFilter(vertices, indexNeighborDict, indicesToUpdate);

		for (int i = 1; i < iterations; i++) {
			newVertices = ComputeLaplacianFilter(newVertices, indexNeighborDict, indicesToUpdate);
		}
		
		return newVertices;
	}

	public static List<Vector3> ComputeLaplacianFilter(List<Vector3> vertices, Dictionary<int, List<int>> indexNeighborDict, HashSet<int> indicesToUpdate = null) {
		List<Vector3> newVertices = new List<Vector3>();

		for (int i = 0; i < vertices.Count; i++) {
			if (indicesToUpdate != null && !indicesToUpdate.Contains(i)) {
				newVertices.Insert(i, vertices[i]);
				continue;
			}

			List<int> neighbors = indexNeighborDict[i];
			
			if (neighbors.Count > 0) {
				Vector3 vertex = vertices[i];

				Vector3 displacement = new Vector3(0f, 0f, 0f);

				for (int j = 0; j < neighbors.Count; j++) {
					int neighborIndex = neighbors[j];
					Vector3 neighbor = vertices[neighborIndex];
					// Debug.Log(string.Format("Neighbor {0}-{1}\n{2}", i, j, neighbor));

					displacement.x += neighbor.x;
					displacement.y += neighbor.y;
					displacement.z += neighbor.z;
				}

				Vector3 average = displacement / neighbors.Count;
				
				newVertices.Insert(i, average);
			}
		}

		return newVertices;
	}
}

