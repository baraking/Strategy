using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="UnitData",menuName = "UnitData")]
public class UnitData : ScriptableObject {

    public PlayerData playerData;
    public int health;
    public int damage;
    public int price;
    public Vector3 highet;
    public float buildTime;

    public float interstRadius;

    public int gatherAmount;
    public float delayTime;

    public Unit[] products;

    public float forwardSpeed;
    public float rotationSpeed;

    public Sprite icon;

}
