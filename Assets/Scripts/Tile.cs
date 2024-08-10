using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject highlight;
    
    private Vector2 gridPosition;

    public void SetGridPosition(Vector2 pos) {
        gridPosition = pos;
    }

    public void Init(bool isOffset) {
        spriteRenderer.color = isOffset ? offsetColor : baseColor;
    }
}