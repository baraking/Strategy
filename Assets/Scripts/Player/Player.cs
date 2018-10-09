using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerData playerData;
    public bool isDefeated;
    public MouseController mouseController;
    public CameraController cameraController;

    public int resources;

    void Start () {
        isDefeated = false;
        mouseController = GetComponent<MouseController>();
        cameraController = GetComponent<CameraController>();
        //playerData = mouseController.playerData;
        resources = 1000;
    }
	
	void Update () {
		
	}

    
}
