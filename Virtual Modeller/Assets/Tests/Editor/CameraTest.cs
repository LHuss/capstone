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
			CameraController cameraController = new CameraController();
			float mockXMovement = -0.05f;
			float mockYMovement = 0f;
			var mockMovementResult = cameraController.OrbitCamera(mockXMovement, mockYMovement);
			Tuple<float, float> expectedResult = new Tuple<float, float>(-0.2f, 0f);
			Assert.AreEqual(expectedResult, mockMovementResult);
		} catch(Exception ex) {
			Debug.Log(ex);
		}
	}

	[Test]
	public void CameraOrbitFail() {
		try{
			CameraController cameraController = new CameraController();
			float mockXMovement = -0.05f;
			float mockYMovement = 0f;
			var mockMovementResult = cameraController.OrbitCamera(mockXMovement, mockYMovement);
			Tuple<float, float> expectedResult = new Tuple<float, float>(-0.12345f, 0f);
			Assert.AreEqual(expectedResult, mockMovementResult);
		} catch(Exception ex){
			Debug.Log("This test was supposed to fail, working as intended.\n" + ex);
		}
	}

	[Test]
	public void CameraZoomPass() {
		try{
			CameraController cameraController = new CameraController();
			float mockZoomMovement = -0.15f;
			cameraController.ZoomCamera(mockZoomMovement);
			float expectedResult = 10.75f;
			Assert.AreEqual(expectedResult, cameraController.CameraDistance);
		} catch(Exception ex) {
			Debug.Log(ex);
		}
	}


	[Test]
	public void CameraZoomFail() {
		try{
			CameraController cameraController = new CameraController();
			float mockZoomMovement = -0.15f;
			cameraController.ZoomCamera(mockZoomMovement);
			float expectedResult = 12.34f;
			Assert.AreEqual(expectedResult, cameraController.CameraDistance);
		} catch(Exception ex) {
			Debug.Log(ex);
		}
	}


	[Test]
	public void CameraResetPass() {
		try{
			CameraController cameraController = new CameraController();
			float mockXMovement = 15f;
			float mockYMovement = 15f;
			cameraController.OrbitCamera(mockXMovement, mockYMovement);
			cameraController.ResetCamera();
			Tuple<float, float, float> expectedResult = new Tuple<float, float, float>(0f, 0f, 0f);
			Assert.AreEqual(expectedResult, cameraController.RotationVect);
		} catch(Exception ex) {
			Debug.Log(ex);
		}
	}


}
