using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapond
{
    public WeapondInfo _info;
    [SerializeField]
    public float _shootDistance;
    [SerializeField]
    public LayerMask _hitLayer;

    private Transform _firePoint;
    private ParticleSystem _muzzleFire;

    [SerializeField]
    private GameObject _hole;

    void Awake()
    {
        _firePoint = transform.Find("FirePoint");
        _muzzleFire = transform.Find("MuzzleFire").GetComponent<ParticleSystem>();
    }

    public override void ChangeToOtherWeapond()
    {
        Destroy(gameObject);
    }

    public override void ChangeToThisWeapond()
    {
        return;
    }

    protected override void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(_firePoint.position, _firePoint.forward, out hit, _shootDistance, _hitLayer))
        {
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<EnemyController>().GetAttack(10, hit.point);
            }
            else
            {
                Instantiate(_hole, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
        }

        _muzzleFire.Play();
    }
}
