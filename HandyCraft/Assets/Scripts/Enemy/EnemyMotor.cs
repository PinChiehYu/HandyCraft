using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotor : MonoBehaviour
{
    private Rigidbody selfRb;
    private Rigidbody[] rbs;

    private Vector3 bodyMovement;

    [SerializeField]
    private float basicSpeed;
    [SerializeField]
    private float speedupSpeed;
    private float currentSpeed;
    [SerializeField, Range(0.1f, 100f)]
    private float rotationSpeed;

    private void Awake()
    {
        selfRb = GetComponent<Rigidbody>();
        rbs = GetComponentsInChildren<Rigidbody>();
        currentSpeed = basicSpeed;
        Ragdoll(false);
    }

    private void FixedUpdate()
    {
        float magnitude = bodyMovement.magnitude;
        if (magnitude != 0) { 
            Quaternion rotation = Quaternion.LookRotation(bodyMovement, Vector3.up);
            selfRb.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, rotationSpeed / 100));
            selfRb.MovePosition(selfRb.position + transform.forward * currentSpeed * Time.fixedDeltaTime);
        }
    }

    public void SetBodyMovement(Vector3 movement)
    {
        bodyMovement = movement;
    }

    public void SpeedUp(bool state)
    {
        currentSpeed = state ? speedupSpeed : basicSpeed;
    }

    public void AddForce(Vector3 direction, float magnitude)
    {
        //rigibody.AddForce(direction * magnitude, ForceMode.Impulse);
    }

    public void Ragdoll(bool state)
    {
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = !state;
        }
    }
}
