using UnityEngine;
using NUnit.Framework;
using System;

public class ModelTest {

	[Test]
	public void SubdivideModelSphere() {
        MeshController meshController = new MeshController();
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.AddComponent<MeshCollider>();

        meshController.AttachMesh(sphere);
        meshController.Model.Start();
        var preSubVertexCount = meshController.Model.vertices.Count;
        meshController.Model.Subdivide();
        var postSubVertexCount = meshController.Model.vertices.Count;
        
        // lower precision to prevent floating point inaccuracy
        Assert.AreEqual(515, preSubVertexCount);
        Assert.AreEqual(1793, postSubVertexCount);
	}

	[Test]
	public void SubdivideModelCube() {
        MeshController meshController = new MeshController();
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.AddComponent<MeshCollider>();

        meshController.AttachMesh(cube);
        meshController.Model.Start();
        var preSubVertexCount = meshController.Model.vertices.Count;
        meshController.Model.Subdivide();
        var postSubVertexCount = meshController.Model.vertices.Count;
        
        // lower precision to prevent floating point inaccuracy
        Assert.AreEqual(24, preSubVertexCount);
        Assert.AreEqual(54, postSubVertexCount);
	}
}
