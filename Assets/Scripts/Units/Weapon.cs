using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Vector3 targetLocation;
    public bool hasATarget;
    public Unit targetUnit;

    Vector3 direction;
    public WeaponData weaponData;
    Quaternion previousRotation;
    public float lastFired;

    public GameObject weaponOriginPoint;

    //public Camera camera;

    void Start () {
        hasATarget = false;
        previousRotation = Quaternion.identity;
        direction = Vector3.one;
    }

	void Update () {
        if (targetUnit != null)
        {
            hasATarget = true;
            targetLocation = targetUnit.transform.position;
        }
        else
        {
            hasATarget = false;
            targetLocation = transform.position + transform.root.transform.forward * 2; 
        }

        if (hasATarget || targetLocation != null) 
        {
            Aim(targetLocation);
        }
    }

    public void Aim(Vector3 target)
    {
        previousRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        direction = Vector3.RotateTowards(transform.forward, (target - transform.position), weaponData.rotationSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    }

    public void Attack(Vector3 target)
    {
        //range check

        targetLocation = target;
        if(weaponData.range >= Vector3.Distance(transform.position, target))
        {
            if (Time.time - lastFired >= weaponData.cooldown && LockedOnEnemy())
            {
                lastFired = Time.time;
                Debug.Log("Pew Pew");
                targetUnit.TakeDanage(weaponData.damage);
            }
        }
        //once aim is good


    }

    public bool LockedOnEnemy()
    {
        //Vector3 aimDirection = targetUnit.transform.position - transform.position;//doesnt consider the highet difference.

        if (targetUnit.Equals(null))
        {
            hasATarget = false;
            return false;
        }

        Vector3 aimDirection = new Vector3(targetUnit.transform.position.x, transform.position.y, targetUnit.transform.position.z) - transform.position;
        float angle = Vector3.Angle(aimDirection, transform.forward);

        if (angle < weaponData.angle * 0.5f)
        {
            //Ray ray = camera.ViewportPointToRay(Vector3.zero);
            RaycastHit hitInfo;


            //Vector3 fwd = transform.TransformDirection(Vector3.forward);
            //Debug.DrawRay(weaponOriginPoint.transform.position, fwd * 50, Color.black);

            //Debug.Log(transform.name + ": " + weaponOriginPoint.transform.position);
            //Origin point is not calculated from the actual canon.
            //maybe add the location of the cannons?
            //distance is now calculated to the center of the target, rather the edge of it.
            if (Physics.Raycast(weaponOriginPoint.transform.position/*transform.position - Vector3.up * 0.9f*/, aimDirection.normalized,out hitInfo,weaponData.beamRaduis*10))
            {
                //Debug.Log(transform.name);
                //Debug.Log(hitInfo.transform.root.name);
                //Debug.Log((hitInfo.transform.root.GetComponent(typeof(Unit)) as Unit).player);
                //Debug.Log((transform.root.GetComponent(typeof(Unit)) as Unit).player);

                //the hit info is probably spotting itself?
                if (hitInfo.collider.transform.root.gameObject.tag=="Unit" /*&& (hitInfo.transform.root.GetComponent(typeof(Unit)) as Unit).player!= (transform.root.GetComponent(typeof(Unit)) as Unit).player*/)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
