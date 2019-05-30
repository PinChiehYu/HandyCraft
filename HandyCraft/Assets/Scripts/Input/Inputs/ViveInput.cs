using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour, IInput
{
    private Transform _cameraTransform;
    private Transform _leftControllerTransform;
    private Transform _rightControllerTransform;

    public SteamVR_Action_Vector2 moveAction;
    public SteamVR_Action_Boolean UIAction;
    public SteamVR_Action_Boolean shootAction;

    private void Start()
    {
        _cameraTransform = GameObject.Find("Camera").transform;
        _leftControllerTransform = GameObject.Find("LeftController").transform;
        _rightControllerTransform = GameObject.Find("RightController").transform;
    }

    public Vector3 GetBodyRotation()
    {
        return new Vector3(0f, _cameraTransform.rotation.eulerAngles.y, 0f);
    }

    public Inputs GetUIOperation()
    {
        if (UIAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            return Inputs.OpenWeapondUI;
        }
        else if (UIAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            return Inputs.OpenOptionUI;
        }

        return Inputs.None;
    }

    public bool GetJump()
    {
        return false;
    }

    public Vector3 GetMovement()
    {
        return new Vector3(moveAction.GetAxis(SteamVR_Input_Sources.LeftHand).x, 0f, moveAction.GetAxis(SteamVR_Input_Sources.LeftHand).y).normalized;
    }

    public bool GetFire(Inputs hand)
    {
        if (shootAction.GetState(GetHandInputSources(hand)))
        {
            return true;
        }

        return false;
    }

    /*
    public Vector3 GetVelocity(Inputs hand)
    {
        return 
    }

    public Vector3 GetAngularVelocity(Inputs hand)
    {

    }
    */

    private SteamVR_Input_Sources GetHandInputSources(Inputs hand)
    {
        return hand == Inputs.RightHand ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), _cameraTransform.rotation.eulerAngles.ToString());
    }
}
