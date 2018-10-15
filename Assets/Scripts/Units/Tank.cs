using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Unit {

    Unit unit;
    Vector3 direction;

    Quaternion previousRotation;

    new void Start()
    {
        base.Start();
        previousRotation = Quaternion.identity;
        direction = Vector3.one;
        unit = GetComponent<Unit>();
    }

    new void Update()
    {
        base.Update();
        target = GetComponent<Unit>().target;
        int caseSwitch = unit.command;
        switch (caseSwitch)
        {
            case 0:
                Move(target);
                break;

            case 1:
                Attack(target);
                break;

            default:
                print("Something went wrong!");
                break;
        }
        
    }

    public override void Move(Vector3 target)
    {
        //1: start and finish turning.
        Turn(unit.target);

        //2:advane forward until close enough.
        if(previousRotation == transform.rotation)
        {
            if (Mathf.Abs(transform.position.x-target.x)>1|| Mathf.Abs(transform.position.z - target.z) > 1)//unit size
            {
                transform.position += transform.forward * unit.unitData.forwardSpeed * Time.deltaTime;
            }
        }

    }

    public void Turn(Vector3 target)
    {
        previousRotation= Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        direction = Vector3.RotateTowards(transform.forward, (target - transform.position), unit.unitData.rotationSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
    }

    //change the attack to a unit perhaps?
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
}
