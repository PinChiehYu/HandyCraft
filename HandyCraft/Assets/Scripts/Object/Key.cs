using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public void Pick(Transform hand)
    {
        GameManager.Instance.Win();
    }

    public void Interact(Vector3 velocity, Vector3 angularVelocity)
    {
        throw new System.NotImplementedException();
    }

    public bool Release(Vector3 velocity, Vector3 angularVelocity)
    {
        Destroy(this);
        return true;
    }
}
