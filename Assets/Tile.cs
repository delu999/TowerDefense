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

    void OnMouseEnter() {
        highlight.SetActive(true);
        // Se la cella è parte di una torretta, evidenzia le 4 celle
        if (highlight.transform.localScale == Vector3.one * 2) {
            highlight.transform.localScale = Vector3.one;
        }
    }

    void OnMouseExit() {
        highlight.SetActive(false);
    }
}