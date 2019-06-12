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
    public float ultRequireTime = 5f;

    private ParticleSystem focusEnergy;

    private void Awake()
    {
        attachedBow = transform.parent;
        positionBuffer = transform.localPosition;
        drawingTimer = 0f;
        focusEnergy = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        focusEnergy.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void Pick(Transform hand)
    {
        drawingHand = hand;
        drawingTimer = 0f;
    }

    private float drawingTimer;
    public void Interact(Vector3 velocity, Vector3 angularVelocity)
    {
        if (drawingHand != null)
        {
            Vector3 handInBowSpace = attachedBow.InverseTransformPoint(drawingHand.position);
            positionBuffer.z = Mathf.Clamp(handInBowSpace.z + arrowCorrection, _minZPosition, _maxZPosition);
            transform.localPosition = positionBuffer;
            drawingTimer += Time.deltaTime;
            if (drawingTimer > ultRequireTime && !focusEnergy.isPlaying)
            {
                focusEnergy.Simulate(0f, true, true);
                focusEnergy.Play();
            }
        }
    }

    public bool Release(Vector3 velocity, Vector3 angularVelocity)
    {
        drawingHand = null;
        attachedBow.GetComponent<Bow>().ShootArrow(drawingTimer > ultRequireTime);
        return true;
    }
}
