using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour 
{

    [SerializeField] private HeatMapVisual heatMapVisual;
    private Grid grid;

    [SerializeField] Transform origin;

    private void Start() 
    {
        grid = new Grid(150, 100, 10f, origin.position);

        heatMapVisual.SetGrid(grid);
    }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            grid.AddValue(position, 100, 2, 25);
        }
    }
}
