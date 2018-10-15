using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour {

    public int value;
    public ResourcesData resourcesData;

	void Start () {
		
	}

    public int Depolt(int amount)
    {
        if (value - amount < 0)
        {
            amount = value;
        }
        value -= amount;
        if (value <= 0)
        {
            Destroy(this.gameObject);
        }

        return amount;
    }

}
