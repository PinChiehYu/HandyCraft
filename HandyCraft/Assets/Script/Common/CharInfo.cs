using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInfo : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int maxHp;

    private int _currentHP;

    public event Action<int> OnHpChange;

    void Start()
    {
        _currentHP = maxHp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public void ReceiveDamage(int damage)
    {
        _currentHP -= damage;
        OnHpChange?.Invoke(_currentHP);
    }
}
