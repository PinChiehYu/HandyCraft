using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private new Rigidbody rigidbody;
    private bool launched;
    public int damage = 10;

    public AudioSource hitAudio;

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
        if (other.transform.root.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            GetStuck(other);
            Destroy(GetComponent<Collider>());
            if (other.transform.root.CompareTag("Enemy"))
            {
                other.GetComponentInParent<IAttackable>().GetAttack(damage, other.transform, transform.position);
            }
            hitAudio.PlayOneShot(hitAudio.clip, 0.3f);
        }
    }

    private void GetStuck(Collider other)
    {
        launched = false;
        rigidbody.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
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
