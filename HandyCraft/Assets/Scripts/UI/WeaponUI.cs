using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text currentWeapond;

    void Start()
    {
        GameManager.Instance.WeapondManager.OnWeapondSwitch += SwitchWeapond;
    }

    private void SwitchWeapond(int id)
    {
        currentWeapond.text = GameManager.Instance.WeapondManager.GetCurrentWeapond().Name;
    }
}
