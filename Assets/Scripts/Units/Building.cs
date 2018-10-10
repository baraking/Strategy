using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit
{
    public Vector3 spawnPoint;
    public Unit myUnit;

    public bool startedBuilding;
    public float buildStartTime;

    new void Start () {
        base.Start();
        spawnPoint = transform.position+new Vector3(0,0,5);
        startedBuilding = false;
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
             if (Input.GetKeyDown(KeyCode.Space))
             {
                if (GameFlowManager.Instance.players[player - 1].resources >= unitData.products[0].unitData.price)
                {
                    GameFlowManager.Instance.players[player - 1].resources -= unitData.products[0].unitData.price;
                    
                    
                    startedBuilding = true;
                    buildStartTime = Time.time;
                    Debug.Log("Building a Unit.");
                }
                else
                {
                    Debug.Log("We don't have enough resources.");
                }
                
             }

            if (startedBuilding)
            {
                if(Time.time-buildStartTime>= unitData.products[0].unitData.buildTime)
                {
                    myUnit = Produce(unitData.products[0], spawnPoint - unitData.highet);
                    Debug.Log("A new unit has been built.");
                    startedBuilding = false;
                }
                
            

            //after the unit is ready
            //myUnit.target = spawnPoint;
            //myUnit.SetTarget(spawnPoint);
            //myUnit.command = (int)Unit.Command.Move;
            }
        }

	}

}
