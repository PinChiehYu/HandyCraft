using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Sword : Weapond
{
    [SerializeField]
    private int damage;

    public override void ChangeToOtherWeapond()
    {
        Destroy(gameObject);
    }

    public override void ChangeToThisWeapond()
    {
        return;
    }

    protected override void Fire(Vector3 velocity, Vector3 angularVelocity)
    {
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Enemy"))
        {
            other.GetComponentInParent<IAttackable>().GetAttack(damage, other.transform, transform.position);
        }
    }
}
