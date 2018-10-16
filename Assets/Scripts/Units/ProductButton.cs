using UnityEngine;
using UnityEngine.UI;

public class ProductButton : MonoBehaviour {

    Unit unit;
    public Image icon;

    public UnitUI unitUI;
    int myIndex;

    public void AddUnit(Unit newUnit, int index)
    {
        unit = newUnit;
        myIndex = index;
        icon.sprite = unit.unitData.icon;
        icon.enabled = true;
    }

    public void RemoveUnit()
    {
        unit = null;
        myIndex = 0;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnButtonPress()
    {
        Debug.Log(unitUI);
        unitUI.selectedUnits.selectedUnits[0].ProduceUnit(myIndex);
    }
}
