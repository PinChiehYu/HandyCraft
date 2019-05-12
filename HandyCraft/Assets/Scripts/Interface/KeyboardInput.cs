using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IInput
{
    private Transform playerTransform;

    public KeyboardInput(Transform transform)
    {
        playerTransform = transform;
    }

    public Inputs GetInputs()
    {
        return Inputs.SwitchWepond;
    }

    public Vector3 GetMovement()
    {
        Vector3 result = Vector3.zero;
        result += Input.GetAxisRaw("Horizontal") * playerTransform.right;
        result += Input.GetAxisRaw("Vertical") * playerTransform.forward;
        result.Normalize();
        return result;
    }

    public Vector3 GetBodyRotation()
    {
        Vector3 result = new Vector3(0f, Input.GetAxisRaw("Mouse X"), 0f);
        return result;
    }

    public Vector3 GetHeadRotation()
    {
        Vector3 result = new Vector3(-1 * Input.GetAxisRaw("Mouse Y"), 0f, 0f);
        return result;
    }

    public bool GetJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool GetFire()
    {
        return Input.GetButton("Fire1");
    }
}
