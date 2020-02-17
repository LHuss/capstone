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
}
