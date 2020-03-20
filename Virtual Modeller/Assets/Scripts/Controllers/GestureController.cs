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
    }

    public void HandleGestures(){
        if (!mvmtCtrl.IsMovementRestricted)
            return;

        try{
            this.frame = this.controller.Frame();
            List<Hand> hands = frame.Hands;
        
            foreach(var hand in frame.Hands){
                if (hand.IsLeft)
                    HandleLeftHand(hand);
                else
                    HandleRightHand(hand);
            }
        }
        catch(NullReferenceException ex){
            Debug.Log("No hands detected: \n" + ex);
        }
    }

    private void HandleLeftHand(Hand left){
        /* Left hand gesture: Fist
        *  Controls rotation of the model 
        */
        if(isFist(left)){
            var pitch = left.PalmNormal.Pitch;
            var roll = left.PalmNormal.Roll;

            if (pitch > -2f && pitch < 3.5f){
                mvmtCtrl.RotateObject(-0.1f, 0, 0);
            }
            else if (pitch < -2.2f){
                mvmtCtrl.RotateObject(0.1f, 0, 0);
            }

            if (roll > -2f && roll < 3.5f){
                mvmtCtrl.RotateObject(0, -0.1f, 0);
            }
            else if (roll < -2.2f){
                mvmtCtrl.RotateObject(0, 0.1f, 0);
            }

            Quaternion q = Quaternion.Euler(mvmtCtrl.RotationVect.x, mvmtCtrl.RotationVect.y, mvmtCtrl.RotationVect.z);
            mvmtCtrl.TransformObject.rotation = Quaternion.Lerp(mvmtCtrl.TransformObject.rotation, q, Time.deltaTime*mvmtCtrl.RotationSpeed);
        }
    }
    private void HandleRightHand(Hand right){
        if(isFist(right)){
            float zoomDist = Time.deltaTime * mvmtCtrl.ZoomSensitivity;
            var pitch = right.PalmNormal.Pitch;

            if (pitch > -2f && pitch < 3.5f){
                mvmtCtrl.ZoomObject(-zoomDist);
            }
            else if (pitch < -2.2f){
                mvmtCtrl.ZoomObject(zoomDist);
            }
            mvmtCtrl.TransformObject.localPosition = new Vector3(
                mvmtCtrl.TransformObject.localPosition.x,
                mvmtCtrl.TransformObject.localPosition.y,
                Mathf.Lerp(
                    mvmtCtrl.TransformObject.localPosition.z,
                    mvmtCtrl.ObjectDistance,
                    Time.deltaTime));
        }
    }

    private bool isFist(Hand hand){
        return !hand.Fingers[0].IsExtended &&
            !hand.Fingers[1].IsExtended &&
            !hand.Fingers[2].IsExtended &&
            !hand.Fingers[3].IsExtended &&
            !hand.Fingers[4].IsExtended;
    }
}