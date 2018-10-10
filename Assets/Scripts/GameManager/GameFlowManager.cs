using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameFlowManager : NetworkBehaviour
{
    #region Singleton
    private static GameFlowManager _instance;

    public static GameFlowManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.Log("Error, multiple singeltons.");
        }
        else
        {
            _instance = this;
        }

        //Fix This -------------------------------------------------------
        //allUnitsByPlayers = new List<Unit>[players.Length];
        Instance.players = new Player[2];
        allUnits = new UnitsOfPlayer[2];
        for (int i = 0; i < 2; i++)
        {
            allUnits[i] = new UnitsOfPlayer();
        }
    }
    #endregion

    public bool isGameOver;
    public Player[] players;

    [System.Serializable] public class UnitsOfPlayer
    {
        public List<Unit> allUnitsOfPlayers = new List<Unit>();
    }
    public UnitsOfPlayer[] allUnits;

    public void Start()
    {
        isGameOver = false;
    }

    public void AddUnitToPlayer(Unit unit, int player)
    {
        //Debug.Log("Unit: " + unit + ", Player: " + player);
        //Debug.Log(allUnits[1].allUnitsOfPlayers);
        allUnits[player - 1].allUnitsOfPlayers.Add(unit);
    }

    public void RemoveUnitFromPlayer(Unit unit, int player)
    {
        allUnits[player - 1].allUnitsOfPlayers.Remove(unit);
    }

    public void Update()
    {
        for(int i = 0; i < allUnits.Length; i++)
        {
            if (allUnits[i].allUnitsOfPlayers.Count < 1)
            {
                Debug.Log("Player number " + (i + 1) + " has lost.");
                //Debug.Log("Player number " + (i + 1) + ","+ players[i].name + ", has lost.");
                //also set that player's isDefetead to True;
            }
        }
    }
}