using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        var gridPosition = new Vector2(
            Mathf.Round(mousePosition.x / gridManager.tileSize), 
            Mathf.Round(mousePosition.y / gridManager.tileSize));
        
        gridManager.PlaceTurret(gridPosition);
    }
}