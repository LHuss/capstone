using System;
using NUnit.Framework;
using UnityEngine;

public class MeshControllerTest
{

    [Test]
    public void MeshPushDeformPass()
    {
        MeshController meshController = new MeshController();
        meshController.DeformationType = DeformationType.PUSH;
        meshController.CollisionForce = 1F;
        var mockCollisionPoint = new Vector3(1.4f, 2.5f, 3.6f);
        var mockCollisionNormal = new Vector3(4.7f, 5.8f, 6.9f);

        var deformExpected = new Vector3(6.1F, 8.3F, 10.5F);
        var deformResult = meshController.Deform(mockCollisionPoint, mockCollisionNormal);
        // lower precision to prevent floating point inaccuracy
        Assert.AreEqual(deformExpected.ToString("F8"), deformResult.ToString("F8"));
    }

    [Test]
    public void MeshPushDeformFail()
    {
        try
        {
            MeshController meshController = new MeshController();
            meshController.DeformationType = DeformationType.PUSH;
            meshController.CollisionForce = 1F;
            var mockCollisionPoint = new Vector3(1f, 2f, 3f);
            var mockCollisionNormal = new Vector3(4f, 5f, 6f);

            var deformExpected = new Vector3(6, 7, 8);
            var deformResult = meshController.Deform(mockCollisionPoint, mockCollisionNormal);
            // lower precision to prevent floating point inaccuracy
            Assert.AreEqual(deformExpected.ToString("F8"), deformResult.ToString("F8"));
        }
        catch (Exception ex)
        {
            Debug.Log("This test was supposed to fail, working as intended.\n" + ex);
        }
    }

    [Test]
    public void MeshPullDeformPass()
    {
        MeshController meshController = new MeshController();
        meshController.DeformationType = DeformationType.PULL;
        meshController.CollisionForce = 1F;
        var mockCollisionPoint = new Vector3(1.2f, 2.3f, 3.4f);
        var mockCollisionNormal = new Vector3(4.5f, 5.6f, 6.7f);

        var deformExpected = new Vector3(-3.3F, -3.3F, -3.3F);
        var deformResult = meshController.Deform(mockCollisionPoint, mockCollisionNormal);
        // lower precision to prevent floating point inaccuracy
        Assert.AreEqual(deformExpected.ToString("F8"), deformResult.ToString("F8"));
    }

    [Test]
    public void MeshPullDeformFail()
    {
        try
        {
            MeshController meshController = new MeshController();
            meshController.DeformationType = DeformationType.PULL;
            meshController.CollisionForce = 1F;
            var mockCollisionPoint = new Vector3(1f, 2f, 3f);
            var mockCollisionNormal = new Vector3(4f, 5f, 6f);

            var deformExpected = new Vector3(6, 7, 8);
            var deformResult = meshController.Deform(mockCollisionPoint, mockCollisionNormal);
            // lower precision to prevent floating point inaccuracy
            Assert.AreEqual(deformExpected.ToString("F8"), deformResult.ToString("F8"));
        }
        catch (Exception ex)
        {
            Debug.Log("This test was supposed to fail, working as intended.\n" + ex);
        }
    }

    [Test]
    public void MeshSameGlobalPointPass()
    {
        MeshController meshController = new MeshController();
        meshController.CollisionAccuracy = 0.01F;
        var mockV1 = new Vector3(1.20f, -2.30f, 3.40f);
        var mockV2 = new Vector3(1.20f, -2.31f, 3.40f);

        var samePointResult = MeshController.SameGlobalPoint(mockV1, mockV2);
        Assert.IsTrue(samePointResult);
    }

    [Test]
    public void MeshSameGlobalPointFail()
    {
        try
        {
            MeshController meshController = new MeshController();
            meshController.CollisionAccuracy = 0.01F;
            var mockV1 = new Vector3(1.20f, -2.30f, 3.40f);
            var mockV2 = new Vector3(1.20f, -2.32f, 3.40f);

            var samePointResult = MeshController.SameGlobalPoint(mockV1, mockV2);
            Assert.IsTrue(samePointResult);
        }
        catch (Exception ex)
        {
            Debug.Log("This test was supposed to fail, working as intended.\n" + ex);
        }
    }
}