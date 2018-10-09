using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "WeaponData")]
public class WeaponData : ScriptableObject
{
    public float rotationSpeed;
    public float damage;
    public float range;
    public float angle;
    public float beamRaduis;
    public float cooldown;
}
