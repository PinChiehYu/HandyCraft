using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapond", menuName = "Weapond")]
public class WeapondInfo : ScriptableObject
{
    public string Name;
    public float Damage;
    public float Range;
    public float Cooldown;
}
