using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private Transform _headTransform;
    private Rigidbody _rigibody;

    private Vector3 _bodyMovement;
    private Vector3 _bodyRotation;
    private Vector3 _headRotation;

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        _headTransform = transform.Find("Head");
        _rigibody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigibody.MovePosition(_rigibody.position + _bodyMovement * movementSpeed * Time.fixedDeltaTime);
        _rigibody.MoveRotation(_rigibody.rotation * Quaternion.Euler(_bodyRotation * rotationSpeed));
        _headTransform.rotation *= Quaternion.Euler(_headRotation * rotationSpeed);
    }

    public void SetBodyMovement(Vector3 movement)
    {
        _bodyMovement = movement;
    }

    public void SetBodyRotation(Vector3 rotation)
    {
        _bodyRotation = rotation;
    }

    public void SetHeadRotation(Vector3 rotation)
    {
        _headRotation = rotation;
    }

    public void Jump()
    {
        _rigibody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
