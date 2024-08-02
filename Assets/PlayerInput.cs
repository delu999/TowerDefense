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
        
        // transpose mouse position
        float x = Mathf.Abs(mousePosition.x - (gridManager.tileSize * gridPosition.x));
        float y = Mathf.Abs(mousePosition.y - (gridManager.tileSize * gridPosition.y));

        //Debug.Log(new Vector2(x, y));
        Debug.Log(new Vector2(mousePosition.x, mousePosition.y));
        
        if (x >= gridManager.tileSize / 2 && y >= gridManager.tileSize / 2)
        {
            gridPosition.y++;
        }
        if (x < gridManager.tileSize / 2 && y >= gridManager.tileSize / 2)
        {
            gridPosition.y++;
            gridPosition.x--;
        }
        if (x < gridManager.tileSize / 2 && y < gridManager.tileSize / 2)
        {
            gridPosition.x--;
        }
        
        gridManager.PlaceTurret(gridPosition);
    }
}