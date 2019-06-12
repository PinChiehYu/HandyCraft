using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    private new Rigidbody rigidbody;
    private bool launched;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        launched = false;
    }

    private void FixedUpdate()
    {
        if (launched && rigidbody.velocity != Vector3.zero)
        {
            rigidbody.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }
    }

    public void Launch()
    {
        launched = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Arrow Hit:" + other.name);
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            GetStuck(other);
            Destroy(GetComponent<Collider>());
            if (other.CompareTag("Enemy"))
            {
                other.GetComponentInParent<IAttackable>().GetAttack(10, other.transform, transform.position);
            }
        }
    }

    private void GetStuck(Collider other)
    {
        launched = false;
        rigidbody.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        //trail.Stop();
        if (other.GetComponent<Rigidbody>())
        {
            gameObject.AddComponent<FixedJoint>().connectedBody = other.GetComponent<Rigidbody>();
        }
        else
        {
            gameObject.AddComponent<FixedJoint>();
        }
    }
}
