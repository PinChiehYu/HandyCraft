using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    private new Rigidbody rigidbody;
    private ParticleSystem trail;
    private bool _launched;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        //trail = GetComponentInChildren<ParticleSystem>();
        _launched = false;
    }

    private void FixedUpdate()
    {
        if (_launched && rigidbody.velocity != Vector3.zero)
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
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
            if (other.CompareTag("Enemy"))
            {
                other.GetComponentInParent<EnemyController>().GetAttack(10, other.transform, transform.position);
            }
        }
    }

    private void GetStuck(Collider other)
    {
        _launched = false;
        rigidbody.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        //trail.Stop();
        gameObject.AddComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();
    }
}
