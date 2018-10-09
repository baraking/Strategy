using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour {

    public GameObject[] unitUI;
    MouseController selectedUnits;
    public int numOfUnits;

    public Texture2D[] icons;
    public GameObject unitIcon;

    void Start()
    {
        selectedUnits = transform.root.GetComponentInChildren(typeof(MouseController)) as MouseController;
    }

	void Update () {
        
        numOfUnits = selectedUnits.selectedUnits.Count;
        foreach (GameObject ui in unitUI)
        {
            ui.SetActive(numOfUnits > 0);
        }
        if (numOfUnits == 1)
        {
            //string name = "Assets/Artwork/" + selectedUnits.selectedUnits[0].unitData.icon.name + ".png";
            //Debug.Log(name);
            //unitIcon = icons[0];
            for(int i = 0; i < icons.Length; i++)
            {
                if (selectedUnits.selectedUnits[0].unitData.icon.name.Equals(icons[i].name))
                {
                    unitIcon.GetComponent<Image>().sprite = Sprite.Create(icons[i], new Rect(0.0f, 0.0f, icons[i].width, icons[i].height), new Vector2(0.5f, 0.5f), 100.0f);
                    return;
                }
            }
            
        }
    }
}
