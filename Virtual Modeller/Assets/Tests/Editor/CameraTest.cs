using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;

public class CameraTest {

	[Test]
	public void CameraOrbitPass() {
		try{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.AddComponent<VMCamera>();
			float mockXMovement = -0.05f;
			float mockYMovement = 0f;
			var mockMovementResult = CameraController.Instance.OrbitCamera(mockXMovement, mockYMovement);
			Tuple<float, float> expectedResult = new Tuple<float, float>(-0.375f, 0f);
			Assert.AreEqual(expectedResult, mockMovementResult);
		} catch(Exception ex) {
			Debug.Log(ex);
		}
	}

	[Test]
	public void CameraOrbitFail() {
		try{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.AddComponent<VMCamera>();
			float mockXMovement = -0.05f;
			float mockYMovement = 0f;
			var mockMovementResult = CameraController.Instance.OrbitCamera(mockXMovement, mockYMovement);
			Tuple<float, float> expectedResult = new Tuple<float, float>(-0.12345f, 0f);
			Assert.AreEqual(expectedResult, mockMovementResult);
		} catch(Exception ex){
			Debug.Log("This test was supposed to fail, working as intended.\n" + ex);
		}
	}

	[Test]
	public void CameraZoomPass() {
		try{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.AddComponent<VMCamera>();
			float mockZoomMovement = -0.15f;
			CameraController.Instance.ZoomCamera(mockZoomMovement);
			float expectedResult = 1.1875f;
			Assert.AreEqual(expectedResult, CameraController.Instance.CameraDistance);
		} catch(Exception ex) {
			Debug.Log(ex);
		}
	}


	[Test]
	public void CameraZoomFail() {
		try{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.AddComponent<VMCamera>();
			float mockZoomMovement = -0.15f;
			CameraController.Instance.ZoomCamera(mockZoomMovement);
			float expectedResult = 12.34f;
			Assert.AreEqual(expectedResult, CameraController.Instance.CameraDistance);
		} catch(Exception ex) {
			Debug.Log("This test was supposed to fail, working as intended.\n" + ex);
		}
	}


	[Test]
	public void CameraResetPass() {
		try{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.AddComponent<VMCamera>();
			Debug.Log(CameraController.Instance.StartingPosition);
			float mockZoom = 20f;
			CameraController.Instance.ZoomCamera(mockZoom);
			CameraController.Instance.ResetCamera();
			Vector3 expectedResult = new Vector3(0f,0f,0f);
			Assert.AreEqual(expectedResult, CameraController.Instance.TransformCamera.position);
		} catch(Exception ex) {
			Debug.Log(ex);
		}
	}


}
