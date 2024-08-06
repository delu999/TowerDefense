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

    private void Update()
    {
        if (_spawnID != -1)
        {
            DetectSpawnPoint();
        }        
    }

    private void DetectSpawnPoint()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        var mousePos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPosDefault = ground.WorldToCell(mousePos);
        var cellPosCentered = ground.GetCellCenterWorld(cellPosDefault);

        // Do not allow to put turret on spawn, base and decorations
        if (ground.GetColliderType(cellPosDefault) 
            != UnityEngine.Tilemaps.Tile.ColliderType.Sprite) return;

        if (CanPlaceTurret(cellPosCentered))
        {
            GameObject g = Instantiate(invisibleTurretPrefab, cellPosCentered, Quaternion.identity);
            if (EnemySpawner.Instance.IsPathAvailable())
            {
                Destroy(g);
                Instantiate(turretsPrefabs[_spawnID], cellPosCentered, Quaternion.identity);
                EnemySpawner.Instance.RecalculatePaths();
                DeselectTowers();
                ground.SetColliderType(cellPosDefault, UnityEngine.Tilemaps.Tile.ColliderType.None);
            }
            else
            {
                Debug.Log("NO");
            }
        }
        else
        {
            Debug.Log("NO");
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
    }
}