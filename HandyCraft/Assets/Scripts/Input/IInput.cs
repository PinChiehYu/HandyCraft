using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    Vector3 GetMovement();
    Vector3 GetBodyRotation();
    bool GetJump();
    Inputs GetUIOperation();
    bool GetFire(Inputs hand);
}
