using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerData",menuName = "PlayerData")]
public class PlayerData : ScriptableObject {

    public new string name;
    public int playerNumber;
    public int playerTeam;
    public Color color;



}
