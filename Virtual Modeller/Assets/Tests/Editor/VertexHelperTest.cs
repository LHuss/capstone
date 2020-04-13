using UnityEngine;
using NUnit.Framework;
using System;
using System.Collections.Generic;

public class VertexHelperTest {
    [Test]
    public void FindNeihboringVertices() {
        Vector3 lineStart = new Vector3(0f, 0f, 0f);
        Vector3 lineMiddle = new Vector3(0f, 0.7f, 0f);
        Vector3 lineEnd = new Vector3(0f, 1f, 0f);
        List<int> triangles = new List<int>{0, 1, 2};
        List<Vector3> vertices = new List<Vector3>{lineStart, lineMiddle, lineEnd};

        List<int> expected = new List<int>{0, 2};
        List<int> actual = VertexHelper.FindNeighboringVertexIndeces(vertices, triangles, lineMiddle);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void IsMatch() {
        Vector3 zero = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 zero2 = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 one = new Vector3(1f, 1f, 1f);

        Assert.True(VertexHelper.IsMatch(zero, zero2));
        Assert.False(VertexHelper.IsMatch(zero, one));
    }

    [Test]
    public void HashVertex() {
        Vector3 vert = new Vector3(0f, 0f, 0f);

        string expectedOne = "0.00.00.0";
        string expectedTwo = "0.000.000.00";

        Assert.AreEqual(expectedOne, VertexHelper.HashVertex(vert, 1));
        Assert.AreEqual(expectedTwo, VertexHelper.HashVertex(vert, 2));
    }
}