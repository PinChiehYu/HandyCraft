using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private Rigidbody _rigibody;

    private Vector3 _bodyMovement;
    private Vector3 _bodyRotation;
    private Vector3 _headRotation;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        _rigibody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigibody.MovePosition(_rigibody.position +  (Quaternion.Euler(_bodyRotation) * _bodyMovement) * movementSpeed * Time.fixedDeltaTime);
    }

    public void SetBodyMovement(Vector3 movement)
    {
        _bodyMovement = movement;
    }

    public void SetBodyRotation(Vector3 rotation)
    {
        _bodyRotation = rotation;
    }

    public void Jump()
    {
        _rigibody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
