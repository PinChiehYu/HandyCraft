using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int maxHp = 100;
    public int MaxHp { get => maxHp; }
    [SerializeField]
    private float maxEnergy = 5f;
    public float MaxEnergy { get => maxEnergy; }

    private int currentHp;
    public int CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = Mathf.Max(value, 0);
            OnHpChange?.Invoke(currentHp);
            if (currentHp == 0)
            {
                OnDie?.Invoke();
            }
        }
    }
    private float currentEnergy;
    public float CurrentEnergy
    {
        get => currentEnergy;
        set
        {
            if (value < currentEnergy) energyTimer = 0f;
            currentEnergy = Mathf.Clamp(value, 0, MaxEnergy);
            OnEnergyChange?.Invoke(currentEnergy);
        }
    }

    public event Action<int> OnHpChange;
    public event Action<float> OnEnergyChange;
    public event Action OnDie;

    [SerializeField]
    private float energyRecoverDelay;
    [SerializeField]
    private float energyRecoverSpeed;
    private float energyTimer;

    private void Awake()
    {
        currentHp = MaxHp;
        currentEnergy = MaxEnergy;
        energyTimer = 0f;
    }

    private void Update()
    {
        UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        if (energyTimer > energyRecoverDelay)
        {
            CurrentEnergy += Time.deltaTime * energyRecoverSpeed;
        }
        else
        {
            energyTimer += Time.deltaTime;
        }
    }

    public void Reset()
    {
        CurrentHp = MaxHp;
        CurrentEnergy = 0;
    }
}
