using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit
{
    public Vector3 spawnPoint;
    public Unit myUnit;

    public bool startedBuilding;
    public float buildStartTime;

    public int index;

    new void Start () {
        base.Start();
        spawnPoint = transform.position+new Vector3(0,0,5);
        startedBuilding = false;
        index = 0;
    }

	new void Update () {
        base.Update();
        //very temporary!!!
        if (isSelected)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                GetResources(50);
            }
        }

        if (startedBuilding)
        {
            if (Time.time - buildStartTime >= unitData.products[index].unitData.buildTime)
            {
                myUnit = Produce(unitData.products[index], spawnPoint - unitData.highet);
                Debug.Log("A new unit has been built.");
                startedBuilding = false;
            }
            //after the unit is ready
            //myUnit.target = spawnPoint;
            //myUnit.SetTarget(spawnPoint);
            //myUnit.command = (int)Unit.Command.Move;
        }

    }

    public override void ProduceUnit(int newIndex)
    {
        if (!startedBuilding)
        {
            if (GameFlowManager.Instance.players[player.playerData.playerNumber - 1].resources >= unitData.products[newIndex].unitData.price)
            {
                GameFlowManager.Instance.players[player.playerData.playerNumber - 1].resources -= unitData.products[newIndex].unitData.price;

                startedBuilding = true;
                buildStartTime = Time.time;
                index = newIndex;
                Debug.Log("Building a Unit.");
            }
            else
            {
                Debug.Log("We don't have enough resources.");
            }
        }
    }

}
