using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void GetAttack(int damage, Transform bodyPart, Vector3 hitPoint);
}
