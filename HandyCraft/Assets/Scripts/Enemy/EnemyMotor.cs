using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotor : MonoBehaviour
{
    private Rigidbody _rigibody;

    private Vector3 _bodyMovement;

    [SerializeField]
    private float movementSpeed;
    [SerializeField, Range(0.1f, 100f)]
    private float rotationSpeed;

    void Start()
    {
        _rigibody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void FixedUpdate()
    {
        float magnitude = _bodyMovement.magnitude;
        Quaternion rotation = Quaternion.LookRotation(_bodyMovement, Vector3.up);
        _rigibody.MoveRotation(Quaternion.Slerp(transform.rotation, rotation, rotationSpeed / 100));
        _rigibody.MovePosition(_rigibody.position + transform.forward * magnitude * movementSpeed * Time.fixedDeltaTime);
    }

    public void SetBodyMovement(Vector3 movement)
    {
        _bodyMovement = movement;
    }

    public void AddForce(Vector3 direction, float magnitude)
    {
        _rigibody.AddForce(direction * magnitude, ForceMode.Impulse);
    }
}
