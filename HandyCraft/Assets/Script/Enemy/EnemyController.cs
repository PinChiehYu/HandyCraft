﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _detectRange;
    private EnemyMotor _motor;

    private Transform _playerTrans;

    void Start()
    {
        _motor = GetComponent<EnemyMotor>();
        _playerTrans = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_playerTrans.position, transform.position) < _detectRange)
        {
            Debug.Log("Detect!");

            Vector3 direction = _playerTrans.position - transform.position;
            direction.y = 0;
            _motor.SetBodyMovement(direction);
        }
        else
        {
            _motor.SetBodyMovement(Vector3.zero);
        }
    }

    public void GetShoot(float damage, Vector3 hitPoint)
    {
        Vector3 stepBack = transform.position - hitPoint;
        stepBack.y = 0;
        stepBack.Normalize();
        _motor.AddForce(stepBack, damage);
    }
}