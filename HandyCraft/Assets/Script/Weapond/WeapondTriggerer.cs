using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeapondTriggerer : MonoBehaviour
{
    // Start is called before the first frame update
    private ParticleSystem _muzzleFire;
    [SerializeField]
    private PlayerShoot _shoot;

    void Start()
    {
        _muzzleFire = GetComponentInChildren<ParticleSystem>();
        _shoot.shootEvent += Trigger;
    }

    // Update is called once per frame
    private void Trigger()
    {
        _muzzleFire.Play();
    }
}
