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
    private AudioSource audio;

    [SerializeField]
    private GameObject hole;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        particles = GetComponentsInChildren<ParticleSystem>();
        audio = GetComponent<AudioSource>();
    }

    private float reloadTimer;
    private void Update()
    {
        reloadTimer += Time.deltaTime;
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
        if (reloadTimer < reloadTime) return;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, shootingRange, hitLayer))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                hit.transform.GetComponentInParent<IAttackable>().GetAttack(10, hit.transform, transform.position);
            }
            else
            {
                Instantiate(hole, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }
        }

        TriggerParticle();
        audio.Play();
        reloadTimer = 0f;
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
