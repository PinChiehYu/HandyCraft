using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody _rigidbody;
    private ParticleSystem trail;
    private bool _launched;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //trail = GetComponentInChildren<ParticleSystem>();
        _launched = false;
    }

    private void FixedUpdate()
    {
        if (_launched && _rigidbody.velocity != Vector3.zero)
        {
            _rigidbody.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
    }

    public void Launch()
    {
        _launched = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Arrow Hit:" + other.name);
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            GetStuck(other);
        }
    }

    private void GetStuck(Collider other)
    {
        _launched = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        //trail.Stop();
        transform.SetParent(other.transform);
    }
}
