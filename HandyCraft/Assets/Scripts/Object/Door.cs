using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Transform root;
    private bool isOpen;
    private bool isProcessing;
    IEnumerator process;

    public float openAngle;
    public float closeAngle;
    public float rotateSpeed;

    private void Awake()
    {
        isOpen = false;
        isProcessing = false;
        openAngle = (openAngle + 360f) % 360;
        closeAngle = (closeAngle + 360f) % 360;
        process = SwitchDoor(false);
    }

    public void Pick(Transform hand)
    {
        Debug.Log("Open");
        if (isProcessing) return;
        isOpen = !isOpen;
        StopCoroutine(process);
        process = SwitchDoor(isOpen);
        StartCoroutine(process);
    }

    public void Interact(Vector3 velocity, Vector3 angularVelocity)
    {
        return;
    }

    public bool Release(Vector3 velocity, Vector3 angularVelocity)
    {
        return true;
    }

    private IEnumerator SwitchDoor(bool open)
    {
        float yRotate = root.rotation.eulerAngles.y;
        float targetAngle = open ? openAngle : closeAngle;
        float diffAngle1 = (yRotate - targetAngle + 360f) % 360f;
        float diffAngle2 = (targetAngle - yRotate + 360f) % 360f;
        float remain = diffAngle1 < diffAngle2 ? diffAngle1 : diffAngle2;
        float sign = remain == diffAngle1 ? -1f : 1f;

        isProcessing = true;
        while (remain - rotateSpeed * Time.deltaTime > 0f)
        {
            root.Rotate(0f, rotateSpeed * sign * Time.deltaTime, 0f, Space.World);
            remain -= rotateSpeed * Time.deltaTime;
            yield return null;
        }
        root.Rotate(0f, remain * sign, 0f);
        isProcessing = false;
    }
}
