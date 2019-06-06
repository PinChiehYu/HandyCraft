using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapond
{
    [SerializeField]
    private float shootingRange;
    [SerializeField]
    private LayerMask hitLayer;
    [SerializeField]
    private float reloadTime;

    private Transform firePoint;
    private ParticleSystem[] particles;

    [SerializeField]
    private GameObject hole;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
    }

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
        if (timer < reloadTime) return;

        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, shootingRange, hitLayer))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponentInParent<EnemyController>().GetAttack(10, hit.transform, hit.point);
            }
            else
            {
                Instantiate(hole, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
        }

        TriggerParticle();
        timer = 0f;
    }

    private void TriggerParticle()
    {
        if (particles.Length == 0) return;
        foreach (ParticleSystem p in particles)
        {
            p.Play();
        }
    }
}
