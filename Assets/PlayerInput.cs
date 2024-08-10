using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask colliderMasks;
    [SerializeField] private List<GameObject> turretsPrefabs;
    [SerializeField] private GameObject invisibleTurretPrefab;
    [SerializeField] private List<Image> turretsUI;
    [SerializeField] private Tilemap ground;
    private int _spawnID = -1;
    
    [SerializeField] private Color previewColorCanPlace = new(0, 1, 0, 0.25f); // Verde semitrasparente
    [SerializeField] private Color previewColorCannotPlace = new(1, 0, 0, 0.25f); // Rosso semitrasparente
    [SerializeField] private GameObject turretPreviewPrefab;
    [SerializeField] private GameObject turretRangePreviewPrefab;
    private GameObject _currentTurretPreview;
    private GameObject _currentTurretRangePreview;

    private void Update()
    {
        if (_spawnID != -1)
        {
            DetectSpawnPoint();
        }        
    }

    private void DetectSpawnPoint()
    {
        if (!Input.GetMouseButtonDown(0)) {
            PreviewTurret();
            return;
        }

        var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPosDefault = ground.WorldToCell(mousePos);
        var cellPosCentered = ground.GetCellCenterWorld(cellPosDefault);

        // Do not allow to put turret on spawn, base and decorations
        if (ground.GetColliderType(cellPosDefault) 
            != UnityEngine.Tilemaps.Tile.ColliderType.Sprite) return;

        var turretCost = turretsPrefabs[_spawnID].GetComponent<Turret>().cost;
        if (CurrencyManager.Instance.CanSpendCurrency(turretCost) && CanPlaceTurret(cellPosCentered))
        {
            GameObject g = Instantiate(invisibleTurretPrefab, cellPosCentered, Quaternion.identity);
            if (EnemySpawner.Instance.IsPathAvailable())
            {
                Destroy(g);
                Instantiate(turretsPrefabs[_spawnID], cellPosCentered, Quaternion.identity);
                CurrencyManager.Instance.SpendCurrency(turretCost);
                EnemySpawner.Instance.RecalculatePaths();
                DeselectTowers();
                ground.SetColliderType(cellPosDefault, UnityEngine.Tilemaps.Tile.ColliderType.None);
            }
            DeselectTowers();
        }
        else
        {
            Debug.Log("NO");
        }
    }
    
    public void PreviewTurret()
    {
        if (Input.GetMouseButtonDown(0)) return;
        
        var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPosDefault = ground.WorldToCell(mousePos);
        var cellPosCentered = ground.GetCellCenterWorld(cellPosDefault);

        if (!ground.HasTile(ground.WorldToCell(cellPosCentered)))
        {
            HideTurretPreview();
            return;
        }
        
        if (_currentTurretPreview is null)
        {
            _currentTurretPreview = Instantiate(turretPreviewPrefab, Vector3.zero, Quaternion.identity);
            _currentTurretRangePreview = Instantiate(turretRangePreviewPrefab, Vector3.zero, Quaternion.identity);
            return;
        }

        bool canPlace = CanPlaceTurret(cellPosCentered);
        _currentTurretPreview.transform.position = cellPosCentered;
        _currentTurretRangePreview.transform.position = _currentTurretPreview.transform.position;
        _currentTurretRangePreview.transform.localScale = new Vector3(turretsPrefabs[_spawnID].GetComponent<Turret>().range, turretsPrefabs[_spawnID].GetComponent<Turret>().range, 1f);
        _currentTurretPreview.SetActive(true);
        _currentTurretRangePreview.SetActive(true);
    
        // Cambia il colore del `SpriteRenderer` in base alla possibilità di posizionamento
        var spriteRenderer = _currentTurretPreview.GetComponent<SpriteRenderer>();
        spriteRenderer.color = canPlace ? previewColorCanPlace : previewColorCannotPlace;
        spriteRenderer = _currentTurretRangePreview.GetComponent<SpriteRenderer>();
        spriteRenderer.color = canPlace ? previewColorCanPlace : previewColorCannotPlace;
    }

    public void HideTurretPreview()
    {
        if (_currentTurretPreview is not null)
        {
            _currentTurretPreview.SetActive(false);
            _currentTurretPreview = null;
            _currentTurretRangePreview.SetActive(false);
            _currentTurretRangePreview = null;
        }
    }

    private bool CanPlaceTurret(Vector3 position)
    {
        return Physics2D.OverlapCircle(position, 0.25f, colliderMasks) is null;
    }
    public void SelectTower(int id)
    {
        DeselectTowers();
        _spawnID = id;
        turretsUI[id].color = new Color(1f, 1f, 1f);
    }

    private void DeselectTowers()
    {
        foreach (var t in turretsUI)
        {
            t.color = new Color(0.4f, 0.4f, 0.4f, 0.6f);
        }
        _spawnID = -1;

        HideTurretPreview();
    }
}