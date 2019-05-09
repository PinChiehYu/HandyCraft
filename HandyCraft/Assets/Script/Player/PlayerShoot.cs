using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // Start is called before the first frame update
    private IInput _input;
    [SerializeField]
    private Camera _camera;
    private AudioSource _audio;
    [SerializeField]
    private GameObject _bulletHole;

    [SerializeField]
    private WeapondInfo _weapon; //Should be assign by another manager
    [SerializeField]
    private LayerMask _hitMask;

    private float _coolDown;

    public event Action OnShoot;

    void Start()
    {
        _input = new KeyboardInput(transform);
        _audio = GetComponent<AudioSource>();
        _coolDown = 0f;
        OnShoot += FireSound;
    }

    // Update is called once per frame
    void Update()
    {
        _coolDown += Time.deltaTime;
        if (_coolDown >= _weapon.Cooldown && _input.GetFire())
        {
            _coolDown = 0f;
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, _weapon.Range, _hitMask))
        {
            if (hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<EnemyController>().GetShoot(_weapon.Damage, hit.point);
            }
            else
            {
                Instantiate(_bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            }

        }

        OnShoot?.Invoke();
    }

    private void FireSound()
    {
        _audio.clip = _weapon.FireSound;
        _audio.Play();
    }
}
