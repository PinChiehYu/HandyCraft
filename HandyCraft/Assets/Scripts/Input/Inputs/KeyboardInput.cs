using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput
{
    public Inputs GetInputs()
    {
        return Inputs.None;
    }

    public Vector3 GetMovement()
    {
        Vector3 result = Vector3.zero;
        result += Input.GetAxis("Horizontal") * Vector3.right;
        result += Input.GetAxis("Vertical") * Vector3.forward;
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

    public Inputs GetFire()
    {
        return Inputs.None;
    }

    public Inputs GetUIOperation()
    {
        return Inputs.None;
    }
}
