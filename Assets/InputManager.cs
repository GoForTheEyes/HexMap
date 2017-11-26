using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class InputManager : MonoBehaviour {

    HexGrid hexGrid;
    HexMapEditor hexMapEditor;
    //HexDirection dragDirection;
    HexCell previousCell, searchFromCell;

    //bool isDrag;

    private void Awake()
    {
        hexGrid = GameObject.FindObjectOfType<HexGrid>();
        hexMapEditor = GameObject.FindObjectOfType<HexMapEditor>();
    }


    void Update()
    {
        if (Input.GetMouseButton(0) &&
            !EventSystem.current.IsPointerOverGameObject() )
        {
            HandleInput(); 
        }
        else if (Input.GetMouseButton(1) &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            CalculatePath();
        }
        else
        {
            previousCell = null;
        }
    }

    void CalculatePath()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            HexCell currentCell = hexGrid.ChangeHex(hit.point);

            if (searchFromCell && searchFromCell != currentCell)
            {
                hexGrid.FindPath(searchFromCell, currentCell);
            }
            else
            {
                searchFromCell = currentCell;
                searchFromCell.EnableHighlight(Color.blue);
            }

        }
        


    }




    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            HexCell currentCell = hexGrid.ChangeHex(hit.point);
            EditCell(currentCell);
            previousCell = currentCell;
        }
        else
        {
            previousCell = null;
        }
    }

    void EditCell(HexCell cell)
    {
        cell.Color = hexMapEditor.ActiveColor;
    }

    //void ValidateDrag(HexCell currentCell)
    //{
    //    for (
    //        dragDirection = HexDirection.NE;
    //        dragDirection <= HexDirection.NW;
    //        dragDirection++
    //    )
    //    {
    //        if (previousCell.GetNeighbor(dragDirection) == currentCell)
    //        {
    //           // isDrag = true;
    //            return;
    //        }
    //    }
    //    //isDrag = false;
    //}


}
