using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class MouseController : NetworkBehaviour
{

    public PlayerData playerData;
    public new Camera camera;
    public int command;//enum from unit

    Vector3 mousePosition1;
    bool isSelecting;

    public Vector3 centerOfSelection;
    public List<Unit> selectedUnits;
    public static int numOfClicks;

    private double timeOfClick;

    // Use this for initialization
    void Start () {
        selectedUnits = new List<Unit>();
        numOfClicks = 0;
        timeOfClick = 0;
        command = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        #region Selection
        centerOfSelection = FindCenterOfSelection();

        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                ClearSelectables();
            }
            mousePosition1 = Input.mousePosition;
        }

        if (isSelecting)
        {
            if (Vector3.Distance(mousePosition1, Input.mousePosition) < 2)
            {
                //no drag box

                if (Physics.Raycast(mouseRay, out hitInfo, 500))
                {
                    Debug.DrawLine(mouseRay.origin, hitInfo.point, Color.yellow);

                    if (hitInfo.transform.root.tag == "Unit" && Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Unit")))
                    {
                        Debug.DrawLine(mouseRay.origin, hitInfo.point, playerData.color);
                        Unit unit = hitInfo.transform.root.GetComponent(typeof(Unit)) as Unit;
                        if (selectedUnits.Contains(unit) && unit.lastClicked != numOfClicks)
                        {
                            unit.isSelected = false;
                            timeOfClick = Time.time;
                            selectedUnits.RemoveAt(RemoveUnit(unit));
                        }
                        else if ((Time.time - timeOfClick) > Time.deltaTime * 5 && !selectedUnits.Contains(unit))
                        {
                            unit.isSelected = true;
                            unit.lastClicked = numOfClicks;
                            selectedUnits.Add(unit);
                            return;
                        }
                    }
                }
            }
            else
            {
                //drag box

                foreach (var hooveredUnit in GameObject.FindGameObjectsWithTag("Unit"))
                {
                    Unit unit = hooveredUnit.GetComponent(typeof(Unit)) as Unit;
                    if (IsWithinSelectionBounds(hooveredUnit))
                    {
                        unit.isSelected = true;
                        unit.lastClicked = numOfClicks;
                        if (!selectedUnits.Contains(unit))
                        {
                            selectedUnits.Add(unit);
                        }
                    }
                    else
                    {
                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            unit.isSelected = false;
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            numOfClicks++;
        }
        #endregion

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(mouseRay, out hitInfo, 500))
            {
                Debug.DrawLine(mouseRay.origin, hitInfo.point, Color.yellow);
                if (hitInfo.transform.root.tag == "Unit" && Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Unit")))
                {
                    Unit possibleTarget = hitInfo.transform.root.GetComponent(typeof(Unit)) as Unit;
                    if (possibleTarget.player != selectedUnits[0].player/*player.playerNumber*/)
                    {
                        foreach (Unit unit in selectedUnits)
                        {
                            unit.target = hitInfo.point;
                            unit.command = (int)Unit.Command.Attack;
                            foreach(Weapon weapon in unit.weapons)
                            {
                                weapon.targetUnit = possibleTarget;
                            }
                        }
                    }
                }
                else if(hitInfo.transform.root.tag == "Resources" && Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Resources")))
                {
                    if(selectedUnits[0] is Worker)
                    {
                        foreach (Worker unit in selectedUnits)
                        {
                            unit.target = hitInfo.point;
                            unit.resourcesTarget = hitInfo.transform.root.GetComponent(typeof(Resources)) as Resources;
                            unit.command = (int)Unit.Command.Gather;
                        }
                    }
                }
                #region Building Spawn Point. Must Improve!
                /*
                //Works as only 1 building is selected and nothing else is.
                else if (selectedUnits[0] is Building)
                {
                    Building building = selectedUnits[0] as Building;
                    building.spawnPoint= hitInfo.point;
                }
                */
                #endregion
                else
                {
                    foreach (Unit unit in selectedUnits)
                    {
                        unit.target = hitInfo.point;
                        unit.command = (int)Unit.Command.Move;
                    }
                }
            }
                
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            var rect = MouseGUI.GetScreenRect(mousePosition1, Input.mousePosition);
            MouseGUI.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            MouseGUI.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    public static Bounds GetViewPortBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = camera.ScreenToViewportPoint(screenPosition1);
        var v2 = camera.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;

    }

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
        {
            return false;
        }

        //var camera = Camera.main;
        var viewportBounds = GetViewPortBounds(camera, mousePosition1, Input.mousePosition);

        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    public Vector3 FindCenterOfSelection()
    {
        Vector3 ans = Vector3.zero;
        if (selectedUnits.Count > 0)
        {
            foreach (Unit unit in selectedUnits)
            {
                ans += unit.gameObject.transform.position;
            }
            ans /= selectedUnits.Count;
        }
        return ans;
    }

    void ClearSelectables()
    {
        foreach (Unit unit in selectedUnits)
        {
            //reset every invidividual unit

            unit.isSelected = false;
            unit.lastClicked = -1;

            //selectedUnits.isSelected = false;
            //selectedUnits.clickOfSelect = -1;
        }
        selectedUnits = new List<Unit>();
    }

    int RemoveUnit(Unit unit)
    {
        int index = 0;
        foreach (Unit tmp in selectedUnits)
        {
            if (tmp.gameObject == unit.gameObject)
            {
                return index;
            }
            index++;
        }
        return -1;
    }
}
