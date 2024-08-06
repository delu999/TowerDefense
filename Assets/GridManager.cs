using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    // [SerializeField] public int numHorizontalTiles;
    // [SerializeField] public int numVerticalTiles;
    // [SerializeField] public float tileSize = 0.5f;
    // [SerializeField] private Tile tilePrefab;
    // [SerializeField] private GameObject placingBlockerPrefab;
    // [SerializeField] private Transform mainCamera;
    // [SerializeField] private Enemy enemyPrefab;
    // [SerializeField] private GameObject turretPrefab;
    // [SerializeField] private LayerMask colliderMask;
    // [SerializeField] private Tilemap spawnTilemap;
    // [SerializeField] private Tilemap baseTilemap;
    //
    // private readonly Dictionary<Vector2, Tile> _tiles = new();
    // private readonly HashSet<Vector2> _occupiedCells = new();
    // private readonly List<Enemy> _enemies = new();
    // private Pathfinding _pathfinding;
    //
    // [SerializeField] private Color previewColorCanPlace = new Color(0, 1, 0, 0.5f); // Verde semitrasparente
    // [SerializeField] private Color previewColorCannotPlace = new Color(1, 0, 0, 0.5f); // Rosso semitrasparente
    // [SerializeField] private GameObject turretPreviewPrefab;
    // private GameObject _currentTurretPreview;
    //
    // public List<Vector3> spawnPositions = new List<Vector3>();
    // public List<Vector3> basePositions = new List<Vector3>();
    
    private void Start()
    {
        EnemySpawner.Instance.StartSpawning();
    }

    // void Start()
    // {
    //     tilePrefab.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
    //     placingBlockerPrefab.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
    //     turretPrefab.transform.localScale = new Vector3(tileSize*2, tileSize*2, tileSize);
    //     turretPreviewPrefab.transform.localScale = turretPrefab.transform.localScale;
    //     _pathfinding = new Pathfinding(numHorizontalTiles, numVerticalTiles, tileSize);
    //     GetTilePositions(spawnTilemap, spawnPositions);
    //     GetTilePositions(baseTilemap, basePositions);
    //     StartCoroutine(SpawnEnemies());
    // }
    //
    // private void GetTilePositions(Tilemap tilemap, List<Vector3> positions)
    // {
    //     foreach (var pos in tilemap.cellBounds.allPositionsWithin)
    //     {
    //         var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
    //         if (!tilemap.HasTile(localPlace)) continue;
    //         var worldPos = tilemap.CellToWorld(localPlace);
    //         positions.Add(worldPos);
    //     }
    // }
    //
    // private IEnumerator SpawnEnemies() {
    //     while (true) {
    //         // Choose a random spawn position from the spawn tilemap
    //         var spawnPosition = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
    //         var spawnedEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    //         spawnedEnemy.Init(_occupiedCells, this);
    //         _enemies.Add(spawnedEnemy);
    //
    //         yield return new WaitForSeconds(2f); // Wait for 2 seconds before spawning the next enemy
    //     }
    // }
    //
    // public void PlaceTurret(Vector2 gridPosition)
    // {
    //     if (!CanPlaceTurret(gridPosition)) return;
    //     
    //     Vector2[] turretCells = {
    //         gridPosition,
    //         gridPosition + new Vector2(1, 0),
    //         gridPosition + new Vector2(0, -1),
    //         gridPosition + new Vector2(1, -1)
    //     };
    //
    //     foreach (var pos in turretCells) {
    //         _occupiedCells.Add(pos);
    //     }
    //
    //     if (IsPathAvailable()) {
    //         if (CurrencyManager.Instance is null || !CurrencyManager.Instance.SpendCurrency(turretPrefab.GetComponent<Turret>().cost))
    //         {
    //             Debug.Log("Not enough currency to place the turret!");
    //             return;
    //         }
    //         
    //         Vector3 turretPosition = gridPosition * tileSize + new Vector2(tileSize / 2, -1 * tileSize / 2);
    //         Instantiate(turretPrefab, turretPosition, Quaternion.identity);
    //         RecalculatePathsForAllEnemies();
    //     } else {
    //         foreach (var pos in turretCells) {
    //             _occupiedCells.Remove(pos);
    //         }
    //         Debug.Log("Cannot place turret, no valid path for enemies.");
    //     }
    // }
    //
    // public void PreviewTurret(Vector2 gridPosition)
    // {
    //     if (_currentTurretPreview is null)
    //     {
    //         _currentTurretPreview = Instantiate(turretPreviewPrefab, Vector3.zero, Quaternion.identity);
    //     }
    //
    //     bool canPlace = CanPlaceTurret(gridPosition);
    //     _currentTurretPreview.transform.position = gridPosition * tileSize + new Vector2(tileSize / 2, - tileSize / 2);
    //     _currentTurretPreview.SetActive(true);
    //
    //     // Cambia il colore del `SpriteRenderer` in base alla possibilità di posizionamento
    //     var spriteRenderer = _currentTurretPreview.GetComponent<SpriteRenderer>();
    //     spriteRenderer.color = canPlace ? previewColorCanPlace : previewColorCannotPlace;
    // }
    //
    // public void HideTurretPreview()
    // {
    //     if (_currentTurretPreview is not null)
    //     {
    //         _currentTurretPreview.SetActive(false);
    //         _currentTurretPreview = null;
    //     }
    // }
    //
    // private bool CanPlaceTurret(Vector2 gridPosition) {
    //     Vector2[] turretCells = {
    //         gridPosition,
    //         gridPosition + new Vector2(1, 0),
    //         gridPosition + new Vector2(0, -1),
    //         gridPosition + new Vector2(1, -1)
    //     };
    //     
    //     foreach (var pos in turretCells) {
    //         if (!_tiles.ContainsKey(pos)) {
    //             Debug.Log("Tile not found: " + pos);
    //             return false;
    //         }
    //
    //         if (IsOccupied(pos)) {
    //             Debug.Log("Position occupied: " + pos);
    //             return false;
    //         }
    //     }
    //     return true;
    // }
    //
    // private bool IsOccupied(Vector2 position)
    // {
    //     position *= tileSize;
    //     return Physics2D.OverlapCircle(position, tileSize / 2, colliderMask) is not null;
    // }
    //
    // private bool IsPathAvailable() {
    //     // Modify this to use basePositions as goal positions
    //     foreach (var goalPosition in basePositions)
    //     {
    //         var startPosition = spawnPositions[0];
    //         if (_pathfinding.FindPath(startPosition, goalPosition, _occupiedCells) != null)
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }
    //
    // private void RecalculatePathsForAllEnemies() {
    //     foreach (var enemy in _enemies) {
    //         enemy.RecalculatePath(_occupiedCells);
    //     }
    // }
    //
    // public void RemoveEnemy(Enemy enemy) {
    //     if (_enemies.Contains(enemy)) {
    //         _enemies.Remove(enemy);
    //     }
    // }
}
