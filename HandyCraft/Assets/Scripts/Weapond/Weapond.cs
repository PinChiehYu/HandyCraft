using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapond : MonoBehaviour, IInteractable
{
    protected Transform hand;
    public abstract void ChangeToThisWeapond();
    public abstract void ChangeToOtherWeapond();
    protected abstract void Fire(Vector3 velocity, Vector3 angularVelocity);

    public void Pick(Transform hand)
    {
        this.hand = hand;
    }

    public void Interact(Vector3 velocity, Vector3 angularVelocity)
    {
        Fire(velocity, angularVelocity);
    }

    public void Release(Vector3 velocity, Vector3 angularVelocity)
    {
    }

    public bool DetachWhenRelease()
    {
        return false;
    }
}
