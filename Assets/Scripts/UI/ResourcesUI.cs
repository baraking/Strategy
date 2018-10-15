using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour {

    Player player;
    int resourcesValue;
    public Text resourcesDisplay;

    void Start () {
        player = transform.root.GetComponent<Player>();
    }

	void Update () {
        UpdateResources(player.resources);
        //resourcesValue = player.resources;
        //resourcesDisplay.text = resourcesValue.ToString();
    }

    public void UpdateResources(int newResources)
    {
        resourcesValue = newResources;
        resourcesDisplay.text = resourcesValue.ToString();
    }
}
