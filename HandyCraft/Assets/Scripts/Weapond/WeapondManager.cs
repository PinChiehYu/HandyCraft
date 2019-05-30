using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeapondManager
{
    public List<WeapondInfo> weapondInfoList;
    private List<int> weapondRemain = new List<int>(); 
    private int currentID;
    private int weapondCount { get => weapondInfoList.Count; }

    public event Action<int> OnWeapondSwitch;

    public WeapondInfo GetCurrentWeapond()
    {
        Debug.Log("Current Weapond:" + weapondInfoList[currentID].Name);
        return weapondInfoList[currentID];
    }

    public WeapondInfo SwitchToNextWeapond()
    {
        currentID = (currentID + 1) % weapondCount;
        OnWeapondSwitch?.Invoke(currentID);
        return GetCurrentWeapond();
    }

    public WeapondInfo SwitchToPreviosWeapdon()
    {
        currentID = (currentID - 1 + weapondCount) % weapondCount;
        OnWeapondSwitch?.Invoke(currentID);
        return GetCurrentWeapond();
    }
}
