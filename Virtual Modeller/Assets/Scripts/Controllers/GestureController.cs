using UnityEngine;
using System;
using Leap;
using Leap.Unity;
using System.Collections.Generic;
public class GestureController : Singleton<GestureController> {
    MovementController mvmtCtrl = MovementController.Instance;
    Controller controller;
    Frame frame;

    void Start(){
        this.controller = new Controller();
        this.frame = controller.Frame();
    }

    public void HandleGestures(){
        this.frame = controller.Frame();
        List<Hand> hands = frame.Hands;
        
        if (frame.Hands.Count > 0 && hands[0].IsLeft &&
                    !hands[0].Fingers[0].IsExtended &&
                    !hands[0].Fingers[1].IsExtended &&
                    !hands[0].Fingers[2].IsExtended &&
                    !hands[0].Fingers[3].IsExtended &&
                    !hands[0].Fingers[4].IsExtended){
            
            var leftPitch = hands[0].PalmNormal.Pitch;
            var leftRoll = hands[0].PalmNormal.Roll;
            var leftYaw = hands[0].PalmNormal.Yaw;

            if (leftPitch > -2f && leftPitch < 3.5f){
                mvmtCtrl.RotateObject(0.1f, 0, 0);
            }
            else if (leftPitch < -2.2f){
                mvmtCtrl.RotateObject(-0.1f, 0, 0);
            }
			Quaternion q = Quaternion.Euler(mvmtCtrl.RotationVect.x, mvmtCtrl.RotationVect.y, mvmtCtrl.RotationVect.z);
			mvmtCtrl.TransformObject.rotation = Quaternion.Lerp(mvmtCtrl.TransformObject.rotation, q, Time.deltaTime*mvmtCtrl.RotationSpeed);
        }
    }
}