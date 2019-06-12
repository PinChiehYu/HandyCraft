using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Pick(Transform hand);
    void Interact(Vector3 velocity, Vector3 angularVelocity);
    bool Release(Vector3 velocity, Vector3 angularVelocity);
}
