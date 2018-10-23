using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Building {

    Unit unit;
    Vector3 direction;

    Quaternion previousRotation;

    public Resources resourcesTarget;
    float lastGather;

    public static int NUM_OF_RESOURCES_AUTO_FIND = 5;

    new void Start () {
        base.Start();
        previousRotation = Quaternion.identity;
        direction = Vector3.one;
        unit = GetComponent<Unit>();
    }

	new void Update () {
        base.Update();
        spawnPoint = transform.position;

        if (startedBuilding)
        {
            command = (int)Command.Build;
            if (Time.time - buildStartTime >= unitData.products[index].unitData.buildTime)
            {
                myUnit = Produce(unitData.products[index], spawnPoint - unitData.highet);
                Debug.Log("A new unit has been built.");
                command = (int)Command.Move;
                startedBuilding = false;
            }
        }

        base.Update();
        target = GetComponent<Unit>().target;
        int caseSwitch = unit.command;
        if (unit.command != (int)Command.Build)
        {
            switch (caseSwitch)
            {
                case 0:
                    Move(target);
                    break;

                case 1:
                    Attack(target);
                    break;

                case 2:
                    Gather(target);
                    break;

                default:
                    print("Something went wrong!");
                    break;
            }
        }
    }

    public override void Move(Vector3 destination)
    {
        //1: start and finish turning.
        Turn(unit.target);

        //2:advane forward until close enough.
        if (previousRotation == transform.rotation)
        {
            if (Mathf.Abs(transform.position.x - target.x) > 1 || Mathf.Abs(transform.position.z - target.z) > 1)//unit size
            {
                transform.position += transform.forward * unit.unitData.forwardSpeed * Time.deltaTime;
            }
        }

    }

    public void Turn(Vector3 target)
    {
        previousRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        direction = Vector3.RotateTowards(transform.forward, (target - transform.position), unit.unitData.rotationSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    }

    public override void Attack(Vector3 target)
    {
        if (minWeaponRange <= Vector3.Distance(transform.position, target))
        {
            Move(target);
        }
        else
        {
            foreach (Weapon weapon in weapons)
            {
                weapon.Attack(target);
            }
        }
    }

    public void Gather(Vector3 target)
    {
        if (resourcesTarget.Equals(null))
        {
            if (FindNearbyResources(transform.position, unitData.interstRadius))
            {
                return;
            }
            else
            {
                OnResourcesDeplot();
            }
        }
        //Debug.Log(Vector3.Distance(transform.position, target));
        //Debug.Log(target);
        if (minWeaponRange <= Vector3.Distance(transform.position, target))
        {
            Move(target);
        }
        else
        {
            //Debug.Log("Attempt to gather");
            if (Time.time - lastGather >= unitData.delayTime)
            {
                lastGather = Time.time;
                int profit = resourcesTarget.Depolt(unitData.gatherAmount);
                if (profit < 0)
                {
                    if (FindNearbyResources(transform.position, unitData.interstRadius))
                    {
                        return;
                    }
                    else
                    {
                        OnResourcesDeplot();
                    }
                }
                Debug.Log("Worker has Gathered " + profit);
                GetResources(profit);
            }
        }
    }

    public void OnResourcesDeplot()
    {
        command = (int)Unit.Command.Move;
        return;
    }

    public bool FindNearbyResources(Vector3 myPosition, float myIntrestRadius)
    {
        List<Resources> possibleResources = new List<Resources>();
        foreach(Resources resource in GameFlowManager.Instance.allResources)
        {
            if (myIntrestRadius >= Vector3.Distance(myPosition, resource.transform.position))
            {
                possibleResources.Add(resource);
            }
            if (possibleResources.Count >= NUM_OF_RESOURCES_AUTO_FIND)
            {
                break;
            }
        }
        if (possibleResources.Count > 0)
        {
            int i = Random.Range(0, possibleResources.Count - 1);
            resourcesTarget = possibleResources[i];
            target = resourcesTarget.transform.position;
            //Debug.Log("Found");
            return true;
        }
        //Debug.Log("Not Found");
        return false;
    }
}
