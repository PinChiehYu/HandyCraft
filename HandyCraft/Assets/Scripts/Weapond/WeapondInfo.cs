using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapond", menuName = "Weapond")]
public class WeapondInfo : ScriptableObject
{
    public string Name;
    public GameObject RightPrefab;
    public GameObject LeftPrefab;
    public Vector3 RightLocalPosition;
    public Vector3 RightLocalRotation;
    public Vector3 LeftLocalPosition;
    public Vector3 LeftLocalRotation;
}
