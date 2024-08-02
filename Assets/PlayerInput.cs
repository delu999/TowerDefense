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
        
        // Calcola l'offset del mouse all'interno del tile
        float offsetX = (gridManager.tileSize / 2 + mousePosition.x) % gridManager.tileSize / gridManager.tileSize;
        float offsetY = (gridManager.tileSize / 2 + mousePosition.y) % gridManager.tileSize / gridManager.tileSize;

        // Determina il quadrante cliccato
        if (offsetX >= gridManager.tileSize / 2 && offsetY >= gridManager.tileSize / 2) {
            // Quadrante in alto a destra
            gridPosition += new Vector2(0, 1);
        } else if (offsetX < gridManager.tileSize / 2 && offsetY >= gridManager.tileSize / 2) {
            // Quadrante in alto a sinistra
            gridPosition += new Vector2(-1, 1);
        } else if (offsetX < gridManager.tileSize / 2 && offsetY < gridManager.tileSize / 2) {
            // Quadrante in basso a sinistra
            gridPosition += new Vector2(-1, 0);
        }
        
        gridManager.PlaceTurret(gridPosition);
    }
}