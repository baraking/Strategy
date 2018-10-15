using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static int NUM_OF_GROUPS = 10;//the 10th group 0 is defualt.

    public PlayerData playerData;
    public bool isDefeated;
    public MouseController mouseController;
    public CameraController cameraController;

    public GameFlowManager.UnitsOfPlayer[] allUnitsByGroup;

    public int resources;

    void Start () {
        isDefeated = false;
        mouseController = GetComponent<MouseController>();
        cameraController = GetComponent<CameraController>();
        //playerData = mouseController.playerData;
        resources = 1000;
        GameFlowManager.Instance.players[playerData.playerNumber - 1] = this;

        allUnitsByGroup = new GameFlowManager.UnitsOfPlayer[NUM_OF_GROUPS];
        for (int i = 0; i < NUM_OF_GROUPS; i++)
        {
            allUnitsByGroup[i] = new GameFlowManager.UnitsOfPlayer();
        }
    }
}
