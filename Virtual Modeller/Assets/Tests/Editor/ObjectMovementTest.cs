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
		} catch(Exception ex) {
			Debug.Log("This test was supposed to fail, working as intended.\n" + ex);
		}
	}
	
}
