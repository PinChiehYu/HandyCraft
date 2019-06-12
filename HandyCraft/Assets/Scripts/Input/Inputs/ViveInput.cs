using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour, IInput
{
    private Transform cameraTransform;
    private Transform leftControllerTransform;
    private Transform rightControllerTransform;

    public SteamVR_Action_Vector2 moveAction;
    public SteamVR_Action_Boolean UIAction;
    public SteamVR_Action_Boolean shootAction;
    public SteamVR_Action_Boolean speedUpAction;
    public SteamVR_Action_Boolean switchLeft, switchRight;

    private void Start()
    {
        cameraTransform = GameObject.Find("Camera").transform;
        leftControllerTransform = GameObject.Find("LeftController").transform;
        rightControllerTransform = GameObject.Find("RightController").transform;
    }

    public Vector3 GetBodyRotation()
    {
        return new Vector3(0f, cameraTransform.rotation.eulerAngles.y, 0f);
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
        return shootAction.GetState(GetHandInputSources(hand));
    }

    public bool GetTriggerSpeedUp()
    {
        return speedUpAction.GetState(SteamVR_Input_Sources.LeftHand);
    }

    public float GetSwitchDirection()
    {
        if (switchRight.GetState(SteamVR_Input_Sources.LeftHand)) return 1f;
        else if (switchLeft.GetState(SteamVR_Input_Sources.LeftHand)) return -1f;
        else return 0f;
    }

    private SteamVR_Input_Sources GetHandInputSources(Inputs hand)
    {
        return hand == Inputs.RightHand ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), cameraTransform.rotation.eulerAngles.ToString());
    }
}
