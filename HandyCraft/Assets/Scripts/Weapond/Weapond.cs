using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapond : MonoBehaviour, IInteractable
{
    public abstract void ChangeToThisWeapond();
    public abstract void ChangeToOtherWeapond();
    protected abstract void Fire();

    public void Pick(Transform hand)
    {
    }

    public void Interact()
    {
        Fire();
    }

    public void Release(Vector3 velocity, Vector3 angularVelocity)
    {
    }

    public bool DetachWhenRelease()
    {
        return false;
    }
}
