using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotor : MonoBehaviour
{
    private Rigidbody rigidbody;

    [SerializeField]
    private float basicSpeed = 0.25f;
    [SerializeField]
    private float speedupSpeed = 1.3f;
    private float currentSpeed;
    [SerializeField, Range(0.1f, 100f)]
    private float rotationSpeed;

    private Vector3 bodyMovement;

    public EnemyState State
    {
        get
        {
            if (bodyMovement.magnitude != 0) return EnemyState.Moving;
            else return EnemyState.Idle;
        }
        private set { }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentSpeed = basicSpeed;
    }

    private void FixedUpdate()
    {
        if (bodyMovement.magnitude != 0) { 
            Quaternion rotation = Quaternion.LookRotation(bodyMovement, Vector3.up);
            rigidbody.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, rotationSpeed / 100));
            rigidbody.MovePosition(rigidbody.position + transform.forward * currentSpeed * Time.fixedDeltaTime);
        }
    }

    public void SetBodyMovement(Vector3 movement)
    {
        bodyMovement = movement;
    }

    public void SetBodyFacing(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    public void SpeedUp(bool state)
    {
        currentSpeed = state ? speedupSpeed : basicSpeed;
    }
}
