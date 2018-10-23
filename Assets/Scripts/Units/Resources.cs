using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour {

    public int value;
    public ResourcesData resourcesData;

    void Start()
    {
        GameFlowManager.Instance.allResources.Add(this);
    }

    public int Depolt(int amount)
    {
        if (this != null)
        { 
            if (value - amount < 0)
            {
                amount = value;
            }
            value -= amount;
            if (value <= 0)
            {
                GameFlowManager.Instance.allResources.Remove(this);
                Destroy(this.gameObject);
            }

            return amount;
        }
        return -1;
    }

}
