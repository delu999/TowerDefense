using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloodFindManager : MonoBehaviour
{
    public static FloodFindManager Instance { get; private set; }

    [SerializeField] private Tilemap ground;
    [SerializeField] private LayerMask enemyMask;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Vector3Int> FloodFind(Vector3Int turretCellPosition)
    {
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Queue<Vector3Int> queue = new Queue<Vector3Int>();
        List<Vector3Int> enemyCells = new List<Vector3Int>();

        queue.Enqueue(turretCellPosition);
        visited.Add(turretCellPosition);

        while (queue.Count > 0)
        {
            Vector3Int currentCell = queue.Dequeue();
            
            if (ContainsEnemy(currentCell))
            {
                enemyCells.Add(currentCell);
            }
            
            foreach (Vector3Int neighbor in GetNeighbors(currentCell))
            {
                if (!visited.Contains(neighbor) && ground.HasTile(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return enemyCells;
    }
    
    private bool ContainsEnemy(Vector3Int cellPosition)
    {
        Vector3 worldPosition = ground.GetCellCenterWorld(cellPosition);
        return Physics2D.OverlapCircle(worldPosition, 0.25f, enemyMask) is not null;
    }
    
    private IEnumerable<Vector3Int> GetNeighbors(Vector3Int current)
    {
        Vector3Int[] cardinalDirections = {
            new(1, 0, 0),  // right
            new(-1, 0, 0), // left
            new(0, 1, 0),  // up
            new(0, -1, 0)  // down
        };
            
        Vector3Int[] diagonalDirections = {
            new(1, 1, 0),   // right-up
            new(1, -1, 0),  // right-down
            new(-1, 1, 0),  // left-up
            new(-1, -1, 0)  // left-down
        };

        foreach (var direction in cardinalDirections)
        {
            var neighbor = current + direction;
            yield return neighbor;
        }

        for (int i = 0; i < diagonalDirections.Length; i++)
        {
            var diagonal = diagonalDirections[i];
            var neighbor = current + diagonal;

            bool isAdjacent1Free = !IsCellOccupied(current + new Vector3Int(diagonal.x, 0, 0));
            bool isAdjacent2Free = !IsCellOccupied(current + new Vector3Int(0, diagonal.y, 0));

            if (isAdjacent1Free && isAdjacent2Free)
            {
                yield return neighbor;
            }
        }
    }
    
    private bool IsCellOccupied(Vector3Int cellPosition)
    {
        return !ground.HasTile(cellPosition);   // This means that cell is walkable by an enemy
    }
    
    public void Restore()
    {
        Instance = null;
    }
}