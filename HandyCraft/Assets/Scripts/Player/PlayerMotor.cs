using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private Rigidbody rigibody;

    private Vector3 bodyMovement;
    private Vector3 bodyRotation;
    private bool isSpeedUp;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float speedUpAmount;

    public PlayerState State {
        get
        {
            if (isSpeedUp && bodyMovement.magnitude > 0f) return PlayerState.Running;
            else if (bodyMovement.magnitude > 0f) return PlayerState.Walking;
            else return PlayerState.Idle;
        }
        private set { }
    }

    private void Awake()
    {
        rigibody = GetComponent<Rigidbody>();
        isSpeedUp = false;
    }

    private void FixedUpdate()
    {
        float speedMulti = isSpeedUp ? speedUpAmount : 1f;
        rigibody.MovePosition(rigibody.position +  (Quaternion.Euler(bodyRotation) * bodyMovement) * movementSpeed * speedMulti * Time.fixedDeltaTime);
    }

    public void SetBodyMovement(Vector3 movement)
    {
        bodyMovement = movement;
    }

    public void SetBodyRotation(Vector3 rotation)
    {
        bodyRotation = rotation;
    }

    public void SpeedUp(bool turnOn)
    {
        isSpeedUp = (turnOn && bodyMovement.z > 0.5f);
    }
}
