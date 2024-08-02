using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] public int numHorizontalTiles; // TODO make it private?
    [SerializeField] public int numVerticalTiles;  // TODO make it private?
    [SerializeField] public float tileSize = 0.5f; // TODO make it private?
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private LayerMask colliderMask;
    
    private readonly Dictionary<Vector2, Tile> _tiles = new();
    private readonly HashSet<Vector2> _occupiedCells = new();
    private readonly List<Enemy> _enemies = new();
    private Pathfinding _pathfinding;
    
    [SerializeField] private Color previewColorCanPlace = new Color(0, 1, 0, 0.5f); // Verde semitrasparente
    [SerializeField] private Color previewColorCannotPlace = new Color(1, 0, 0, 0.5f); // Rosso semitrasparente
    [SerializeField] private GameObject turretPreviewPrefab;
    private GameObject _currentTurretPreview;
    
    void Start()
    {
        tilePrefab.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
        turretPrefab.transform.localScale = new Vector3(tileSize*2, tileSize*2, tileSize);
        turretPreviewPrefab.transform.localScale = turretPrefab.transform.localScale;
        _pathfinding = new Pathfinding(numHorizontalTiles, numVerticalTiles, tileSize);
        GenerateGrid();
        CenterCamera();
        StartCoroutine(SpawnEnemies());
    }

    private void GenerateGrid() {
        for (var x = 0; x < numHorizontalTiles; x++) {
            for (var y = 0; y < numVerticalTiles; y++) {
                var xPos = x * tileSize;
                var yPos = y * tileSize;

                var spawnedTile = Instantiate(tilePrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.SetGridPosition(new Vector2(x, y));

                var isOffset = (x + y) % 2 == 1;
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }

    private void CenterCamera() {
        var centerX = (numHorizontalTiles * tileSize) / 2 - (tileSize / 2);
        var centerY = (numVerticalTiles * tileSize) / 2 - (tileSize / 2);
        mainCamera.position = new Vector3(centerX, centerY, -10);
    }

    private IEnumerator SpawnEnemies() {
        while (true) {
            var randomSpawnRow = Random.Range(0, numVerticalTiles - 1);
            var spawnPosition = new Vector2(0, randomSpawnRow * tileSize);
            var spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            spawnedEnemy.Init(_occupiedCells, this);
            _enemies.Add(spawnedEnemy);

            yield return new WaitForSeconds(2f); // Wait for 2 seconds before spawning the next enemy
        }
    }

    public void PlaceTurret(Vector2 gridPosition)
    {
        if (!CanPlaceTurret(gridPosition)) return;
        
        Vector2[] turretCells = {
            gridPosition,
            gridPosition + new Vector2(1, 0),
            gridPosition + new Vector2(0, -1),
            gridPosition + new Vector2(1, -1)
        };

        foreach (var pos in turretCells) {
            _occupiedCells.Add(pos);
        }

        if (IsPathAvailable()) {
            Vector3 turretPosition = gridPosition * tileSize + new Vector2(tileSize / 2, -1 * tileSize / 2);
            Instantiate(turretPrefab, turretPosition, Quaternion.identity);
            RecalculatePathsForAllEnemies();
        } else {
            foreach (var pos in turretCells) {
                _occupiedCells.Remove(pos);
            }
            Debug.Log("Cannot place turret, no valid path for enemies.");
        }
    }

    public void PreviewTurret(Vector2 gridPosition)
    {
        if (_currentTurretPreview is null)
        {
            _currentTurretPreview = Instantiate(turretPreviewPrefab, Vector3.zero, Quaternion.identity);
        }

        bool canPlace = CanPlaceTurret(gridPosition);
        _currentTurretPreview.transform.position = gridPosition * tileSize + new Vector2(tileSize / 2, - tileSize / 2);
        _currentTurretPreview.SetActive(true);
    
        // Cambia il colore del `SpriteRenderer` in base alla possibilità di posizionamento
        var spriteRenderer = _currentTurretPreview.GetComponent<SpriteRenderer>();
        spriteRenderer.color = canPlace ? previewColorCanPlace : previewColorCannotPlace;
    }

    public void HideTurretPreview()
    {
        if (_currentTurretPreview is not null)
        {
            _currentTurretPreview.SetActive(false);
            _currentTurretPreview = null;
        }
    }

    private bool CanPlaceTurret(Vector2 gridPosition) {
        Vector2[] turretCells = {
            gridPosition,
            gridPosition + new Vector2(1, 0),
            gridPosition + new Vector2(0, -1),
            gridPosition + new Vector2(1, -1)
        };
        
        foreach (var pos in turretCells) {
            if (!_tiles.ContainsKey(pos)) {
                Debug.Log("Tile not found: " + pos);
                return false;
            }

            if (IsOccupied(pos)) {
                Debug.Log("Position occupied: " + pos);
                return false;
            }
        }
        return true;
    }

    private bool IsOccupied(Vector2 position)
    {
        position *= tileSize;
        return Physics2D.OverlapCircle(position, tileSize / 2, colliderMask) is not null;
    }

    private bool IsPathAvailable() {
        var startPosition = new Vector2(0, 0);
        var goalPosition = new Vector2((numHorizontalTiles - 1) * tileSize, 0);

        return _pathfinding.FindPath(startPosition, goalPosition, _occupiedCells) != null;
    }

    private void RecalculatePathsForAllEnemies() {
        foreach (var enemy in _enemies) {
            enemy.RecalculatePath(_occupiedCells);
        }
    }

    public void RemoveEnemy(Enemy enemy) {
        if (_enemies.Contains(enemy)) {
            _enemies.Remove(enemy);
        }
    }
}
