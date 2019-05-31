using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowInBow : MonoBehaviour, IInteractable
{
    public float _minZPosition;
    public float _maxZPosition;

    private Transform attachedBow; // 3
    private Vector3 positionBuffer;
    private Transform drawingHand;
    private Vector3 cachedTransform;
    public const float arrowCorrection = 0.35f;

    private void Awake()
    {
        attachedBow = transform.parent;
        positionBuffer = transform.localPosition;
    }

    public void Pick(Transform hand)
    {
        drawingHand = hand;
    }

    public void Interact()
    {
        if (drawingHand != null)
        {
            Vector3 handInBowSpace = attachedBow.InverseTransformPoint(drawingHand.position);
            positionBuffer.z = Mathf.Clamp(handInBowSpace.z + arrowCorrection, _minZPosition, _maxZPosition);
            transform.localPosition = positionBuffer;
        }
    }

    public void Release(Vector3 velocity, Vector3 angularVelocity)
    {
        attachedBow.GetComponent<Bow>().ShootArrow();
        drawingHand = null;
    }

    public bool DetachWhenRelease()
    {
        return true;
    }
}
