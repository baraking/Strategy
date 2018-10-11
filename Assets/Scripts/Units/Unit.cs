using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class Unit : MonoBehaviour/*NetworkBehaviour*/ {

    public UnitData unitData;
    public int player;

    public bool isSelected;
    public Vector3 target;
    public enum Command { Move, Attack, Gather };

    public Weapon[] weapons;

    public int command;
    public int lastClicked;

    public float minWeaponRange;

    //[SyncVar(hook = "TakeDanage")]
    public float health;
    public bool isDestroyed;

    HealthBar healthBar;

    public System.Action OnUnitDeath;

    public void Start()
    {
        //add to player's list
        GameFlowManager.Instance.AddUnitToPlayer(this, player);

        target = transform.position;
        minWeaponRange = FindMinRange(weapons);
        health = unitData.health;
        OnUnitDeath += UnitDeath;
        healthBar = GetComponentInChildren<HealthBar>();
    }

    public void Update()
    {
        if (health <= 0)
        {
            OnUnitDeath();
        }
    }

    public virtual void Move(Vector3 target)
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
        if (weapons.Length < 1)
        {
            //a weaponless/meele unit
            return 0;
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
        return Instantiate(unit, target+unit.unitData.highet, Quaternion.identity);
    }

    void UnitDeath()
    {
        isDestroyed = true;
        Debug.Log(gameObject.name + " Has died.");
        Destroy(transform.root.gameObject);
        //remove from player's list
        GameFlowManager.Instance.RemoveUnitFromPlayer(this, player);
    }

    public void GetResources(int amount)
    {
        GameFlowManager.Instance.players[player - 1].resources += amount;
    }
}
