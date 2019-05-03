using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    Inputs GetInputs();
    Vector3 GetMovement();
    Vector3 GetBodyRotation();
    Vector3 GetHeadRotation();
}
