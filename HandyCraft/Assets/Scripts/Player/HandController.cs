using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(SteamVR_Behaviour_Pose))]
public class HandController : MonoBehaviour
{
    [SerializeField]
    private Inputs _handType;
    private IInteractable holdingObject;

    private IInput input;

    private GameObject touchingObject;
    private bool isTouchingObject { get { return touchingObject != null; } }

    private SteamVR_Behaviour_Pose controllerPose;
    private GameObject handModel;

    private void Awake()
    {
        controllerPose = GetComponent<SteamVR_Behaviour_Pose>();
        holdingObject = null;
        touchingObject = null;
        handModel = transform.Find("Model").gameObject;
    }

    private void Start()
    {
        input = GameManager.Instance.GetInputSource();
    }

    private void Update()
    {
        if (IsGrabing())
        {
            if (holdingObject != null)
            {
                holdingObject.Interact();
            }
            else if (isTouchingObject)
            {
                HoldObject();
            }
        }
        else
        {
            ReleaseObject();
        }
    }

    public void SwitchWeapond(GameObject prefab, Vector3 localPosition, Vector3 localRotation)
    {
        //clear old weapond
        if (holdingObject is Weapond weapond)
        {
            Debug.Log(_handType.ToString() + ":" + weapond.gameObject.name);
            weapond.ChangeToOtherWeapond();
            holdingObject = null;
        }
        else if (holdingObject != null)
        {
            return;
        }

        if (prefab != null)
        {
            SetHandModelDisplay(true);
            GameObject instance = Instantiate(prefab);
            instance.transform.SetParent(transform);
            instance.transform.localPosition = localPosition;
            instance.transform.localRotation = Quaternion.Euler(localRotation);
            if (instance.GetComponent<Weapond>() == null) {
                Debug.LogWarning("Didn't find Weapond component on this weapond. Please correct this problem.");
            }
            else
            {
                holdingObject = instance.GetComponent<Weapond>();
                (holdingObject as Weapond).ChangeToThisWeapond();
            }
        }
        else
        {
            SetHandModelDisplay(true);
        }
    }

    private bool IsGrabing()
    {
        return input.GetFire(_handType);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryTouchObject(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryTouchObject(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (touchingObject == other.gameObject)
        {
            Debug.Log(_handType.ToString() + " leave " + other.name);
            touchingObject = null;
        }
    }

    private void TryTouchObject(Collider col)
    {
        if (holdingObject == null && touchingObject == null && col.GetComponent<IInteractable>() != null)
        {
            Debug.Log(_handType.ToString() + " touch " + col.name);
            touchingObject = col.gameObject;
        }
    }

    private void HoldObject()
    {
        holdingObject = touchingObject.GetComponent<IInteractable>();
        if (holdingObject != null)
        {
            holdingObject.Pick(transform);
            touchingObject = null;
        }
    }

    private void ReleaseObject()
    {
        if (holdingObject != null)
        {
            holdingObject.Release(controllerPose.GetVelocity(), controllerPose.GetAngularVelocity());
            if (holdingObject.DetachWhenRelease())
            {
                Debug.Log("Detach Holding Object");
                holdingObject = null;
            }
        }
    }

    private void SetHandModelDisplay(bool active)
    {
        handModel.SetActive(active);
    }
}
