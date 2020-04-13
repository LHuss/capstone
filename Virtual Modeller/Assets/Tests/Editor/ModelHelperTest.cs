using UnityEngine;
using NUnit.Framework;
using System;
using System.Collections.Generic;

public class ModelHelperTest {
    [Test]
    public void ComputeLaplacianFilter() {
        Vector3 lineStart = new Vector3(0f, 0f, 0f);
        Vector3 lineMiddle = new Vector3(0f, 0.7f, 0f);
        Vector3 lineEnd = new Vector3(0f, 1f, 0f);
        Dictionary<int, List<int>> indexNeighborDict = new Dictionary<int, List<int>>();
        List<int> middleNeighbors = new List<int>{0, 2};
        indexNeighborDict.Add(1, middleNeighbors);
        List<Vector3> vertices = new List<Vector3>{lineStart, lineMiddle, lineEnd};

        Vector3 expectedNewMiddle = new Vector3(0f, 0.5f, 0f);
        List<Vector3> expectedNewVertices = new List<Vector3>{lineStart, expectedNewMiddle, lineEnd};
        HashSet<int> indicesToUpdate = new HashSet<int>{1};

        List<Vector3> actualNewVertices = ModelHelper.ComputeLaplacianFilter(vertices, indexNeighborDict, indicesToUpdate);
        Assert.AreEqual(expectedNewVertices, actualNewVertices);
    }
}