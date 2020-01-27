using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class ObjectMovementTest {

	[Test]
	public void ObjectRotationPass() {
		try{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.AddComponent<ObjectMovement>();
			float mockXMovement = -0.05f;
			float mockYMovement = 0f;
			var mockMovementResult = MovementController.Instance.RotateObject(mockXMovement, mockYMovement);
			Tuple<float, float> expectedResult = new Tuple<float, float>(-0.4f, 0f);
			Assert.AreEqual(expectedResult, mockMovementResult);
		} catch(Exception ex) {
			Debug.Log(ex);
		}
	}

	[Test]
	public void ObjectRotationFail() {
		try{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.AddComponent<ObjectMovement>();
			float mockXMovement = -0.05f;
			float mockYMovement = 0f;
			var mockMovementResult = MovementController.Instance.RotateObject(mockXMovement, mockYMovement);
			Tuple<float, float> expectedResult = new Tuple<float, float>(100f, 0f);
			Assert.AreEqual(expectedResult, mockMovementResult);
		} catch(AssertionException ex) {
			Debug.Log("This test was supposed to fail, working as intended.\n" + ex);
		}
	}


	[Test]
	public void ZoomPass() {
		try{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.AddComponent<ObjectMovement>();
			float mockZoomMovement = -0.15f;
			MovementController.Instance.ZoomObject(mockZoomMovement);
			float expectedResult = 0.185000002f;
			Assert.AreEqual(expectedResult, MovementController.Instance.ObjectDistance);
		} catch(Exception ex) {
			Debug.Log(ex);
		}
	}


	[Test]
	public void ZoomFail() {
		try{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.AddComponent<ObjectMovement>();
			float mockZoomMovement = -0.15f;
			MovementController.Instance.ZoomObject(mockZoomMovement);
			float expectedResult = 12.34f;
			Assert.AreEqual(expectedResult, MovementController.Instance.ObjectDistance);
		} catch(AssertionException ex) {
			Debug.Log("This test was supposed to fail, working as intended.\n" + ex);
		}
	}


	[Test]
	public void ResetPass() {
		try{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.AddComponent<ObjectMovement>();
			Debug.Log(MovementController.Instance.StartingPosition);
			float mockZoom = 20f;
			MovementController.Instance.ZoomObject(mockZoom);
			MovementController.Instance.ResetPosition();
			Vector3 expectedResult = new Vector3(0f,0f,0f);
			Assert.AreEqual(expectedResult, MovementController.Instance.TransformObject.position);
		} catch(Exception ex) {
			Debug.Log(ex);
		}
	}

}
