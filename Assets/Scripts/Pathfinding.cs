using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding
{
    private readonly LayerMask _colliderMasks;
    private readonly Tilemap _tilemap; // Aggiungi la tilemap

    public Pathfinding(LayerMask colliderMasks, Tilemap tilemap)
    {
        _colliderMasks = colliderMasks;
        _tilemap = tilemap;
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 goal)
    {
        var startCell = _tilemap.WorldToCell(start);
        var goalCell = _tilemap.WorldToCell(goal);

        PriorityQueue<Vector3Int> openSet = new();
        openSet.Enqueue(startCell, 0);

        Dictionary<Vector3Int, Vector3Int> cameFrom = new();
        Dictionary<Vector3Int, float> gScore = new() { [startCell] = 0 };
        Dictionary<Vector3Int, float> fScore = new() { [startCell] = Heuristic(startCell, goalCell) };

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current == goalCell)
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (var neighbor in GetNeighbors(current))
            {
                if (IsCellOccupied(neighbor)) continue;

                var tentativeGScore = gScore[current] + Vector3Int.Distance(current, neighbor);

                if (gScore.ContainsKey(neighbor) && !(tentativeGScore < gScore[neighbor])) continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goalCell);
                openSet.Enqueue(neighbor, fScore[neighbor]);
            }
        }

        return null; // No path found
    }
    private List<Vector2> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        while (cameFrom.ContainsKey(current))
        {
            if (current.x != cameFrom[current].x) break;
            current = cameFrom[current];
        }
        
        var path = new List<Vector2> { _tilemap.GetCellCenterWorld(current) };
        bool targetXReached = false;
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(_tilemap.GetCellCenterWorld(current));
        }

        if (path.Count > 1)
        {
            path.Reverse();
            path.RemoveAt(0);
        }
        return path;
    }

    private float Heuristic(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private IEnumerable<Vector3Int> GetNeighbors(Vector3Int current)
    {
        Vector3Int[] directions = {
            new(1, 0, 0), // right
            new(-1, 0, 0), // left
            new(0, 1, 0), // up
            new(0, -1, 0) // down
        };
        


        foreach (var direction in directions)
        {
            var neighbor = current + direction;
            yield return neighbor;
        }
    }

    private bool IsCellOccupied(Vector3Int cellPosition)
    {
        Vector3 worldPosition = _tilemap.GetCellCenterWorld(cellPosition);

        if (!_tilemap.HasTile(cellPosition))
        {
            return true;
        }

        return Physics2D.OverlapCircle(worldPosition, 0.25f, _colliderMasks) != null;
    }
}
