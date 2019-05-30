using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowInBow : MonoBehaviour, IInteractable
{
    public float _minZPosition; // 1
    public float _maxZPosition; // 2

    private Transform _attachedBow; // 3
    private Transform drawingHand;
    private Vector3 cachedTransform;
    private const float arrowCorrection = 0.3f;

    private void Awake()
    {
        _attachedBow = transform.parent;
    }

    public void Pick(Transform hand)
    {
        drawingHand = hand;
    }

    public void Interact()
    {
        if (drawingHand != null)
        {
            Vector3 handInBowSpace = _attachedBow.InverseTransformPoint(drawingHand.position);
            float zPos = Mathf.Clamp(handInBowSpace.z + arrowCorrection, _minZPosition, _maxZPosition);
            transform.localPosition = new Vector3(0, 0, zPos);
        }
    }

    public void Release(Vector3 velocity, Vector3 angularVelocity)
    {
        _attachedBow.GetComponent<Bow>().ShootArrow();
        drawingHand = null;
    }

    public bool DetachWhenRelease()
    {
        return true;
    }
}
