using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class Unit : MonoBehaviour/*NetworkBehaviour*/ {

    public UnitData unitData;
    public Player player;

    public bool isSelected;
    public Vector3 target;
    public enum Command { Move, Attack, Gather, Build }
    public enum Behavior { CommandOnly, Reactive, Aggresive}

    public Weapon[] weapons;

    public int command;
    public int lastClicked;
    public int group;

    public float minWeaponRange;

    //[SyncVar(hook = "TakeDanage")]
    public float health;
    public bool isDestroyed;

    HealthBar healthBar;

    public static int NUM_OF_TARGETS_AUTO_FIND = 3;

    public System.Action OnUnitDeath;

    public void Start()
    {
        //add to player's list
        GameFlowManager.Instance.AddUnitToPlayer(this, player.playerData.playerNumber);
        target = transform.position;
        minWeaponRange = FindMinRange(weapons);
        health = unitData.health;
        OnUnitDeath += UnitDeath;
        healthBar = GetComponentInChildren<HealthBar>();
        SetGroupNumber(0);
    }

    public void Update()
    {
        if (health <= 0)
        {
            OnUnitDeath();
        }
    }

    public void SetGroupNumber(int gruopNumber)
    {
        player.allUnitsByGroup[group].allUnitsOfPlayers.Remove(this);
        group = gruopNumber;
        healthBar.SetGroupNumberUI(group);
        player.allUnitsByGroup[group].allUnitsOfPlayers.Add(this);
    }

    public virtual void Move(Vector3 destination)
    {

    }

    public virtual void Attack(Vector3 target)
    {

    }

    public void SetTarget(Vector3 target)
    {
        target = this.target;
    }

    public float FindMinRange(Weapon[] weapons)
    {
        if (weapons == null || weapons.Length < 1) 
        {
            //a weaponless/meele unit
            return 2f;
        }
        float ans = weapons[0].weaponData.range;
        for(int i = 1; i < weapons.Length; i++)
        {
            if (weapons[i].weaponData.range < ans)
            {
                ans = weapons[i].weaponData.range;
            }
        }
        return ans;
    }

    public void TakeDanage(float damage)
    {
        health -= damage;
        healthBar.filledHealthBar.fillAmount = health / unitData.health;
    }

    public Unit Produce(Unit unit, Vector3 target)
    {
        Unit newUnit= Instantiate(unit, target+unit.unitData.highet, Quaternion.identity);
        newUnit.player = this.player;
        return newUnit;
    }

    public virtual void ProduceUnit(int index)
    {
        Debug.Log("I have no units. try doing this for a building");
    }

    void UnitDeath()
    {
        isDestroyed = true;
        Debug.Log(gameObject.name + " Has died.");
        Destroy(transform.root.gameObject);
        //remove from player's list
        GameFlowManager.Instance.RemoveUnitFromPlayer(this, player.playerData.playerNumber);
    }

    public void GetResources(int amount)
    {
        GameFlowManager.Instance.players[player.playerData.playerNumber - 1].resources += amount;
    }
}
