using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int maxHp;

    private int currentHP;

    public event Action<int> OnHpChange;
    public event Action OnDie;

    private void Awake()
    {
        currentHP = maxHp;
    }

    public int GetMaxHp()
    {
        return maxHp;
    }

    public void ReceiveDamage(int damage)
    {
        currentHP = Mathf.Max(currentHP - damage, 0);
        OnHpChange?.Invoke(currentHP);
        if (currentHP == 0)
        {
            OnDie?.Invoke();
        }
    }
}
