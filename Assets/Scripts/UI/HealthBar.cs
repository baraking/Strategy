using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Camera cameraToLookAt;

    //public static float filled = 1f;
    //public static float empty = 0.0f;

    public Image filledHealthBar;
    public GameObject emptyHealthBar;
    public Text groupNumber;

    public Unit unit;

    float maxHealth;

    void Start () {
        
        maxHealth = unit.unitData.health;

        cameraToLookAt = FindObjectOfType<Camera>();
        SetGroupNumberUI(unit.group);
    }

	void Update () {
        


        //at the moment, it is facing the camera.
        //should be paradicular to it instead.
        #region CameraFacing
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0;
        transform.LookAt(cameraToLookAt.transform.position - v);
        transform.Rotate(0, 180, 0);
        #endregion
    }

    public void SetGroupNumberUI(int newGroupNumber)
    {
        groupNumber.text = newGroupNumber.ToString();
    }
}
