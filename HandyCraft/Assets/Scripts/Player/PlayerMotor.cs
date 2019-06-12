using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private Rigidbody rigibody;

    private Vector3 bodyMovement;
    private Vector3 bodyRotation;
    private float speedMulti;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float speedUpAmount;
    [SerializeField]
    private float jumpForce;

    private void Awake()
    {
        rigibody = GetComponent<Rigidbody>();
        speedMulti = 1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        if (turnOn && bodyMovement.z > 0.5f) // movement between +- 60 degree
        {
            speedMulti = speedUpAmount;
        }
        else
        {
            speedMulti = 1f;
        }
    }

    public void Jump()
    {
        rigibody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
