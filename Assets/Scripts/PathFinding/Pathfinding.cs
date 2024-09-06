using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathFinding
{
    public class Pathfinding
    {
        private readonly LayerMask _colliderMasks;
        private readonly Tilemap _tilemap;

        public Pathfinding(LayerMask colliderMasks, Tilemap tilemap)
        {
            _colliderMasks = colliderMasks;
            _tilemap = tilemap;
        }

        public List<Vector2> FindPath(Vector2 start, List<Vector2> goals)
        {
            var startCell = _tilemap.WorldToCell(start);
            var goalCells = new List<Vector3Int>();
            foreach (var goal in goals)
            {
                goalCells.Add(_tilemap.WorldToCell(goal));
            }

            PriorityQueue<Vector3Int> openSet = new();
            openSet.Enqueue(startCell, 0);

            Dictionary<Vector3Int, Vector3Int> cameFrom = new();
            Dictionary<Vector3Int, float> gScore = new() { [startCell] = 0 };
            Dictionary<Vector3Int, float> fScore = new() { [startCell] = Heuristic(startCell, goalCells) };

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (goalCells.Contains(current))
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
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goalCells);
                    openSet.Enqueue(neighbor, fScore[neighbor]);
                }
            }

            return null; // No path found
        }

        private List<Vector2> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
        {
            var path = new List<Vector2> { _tilemap.GetCellCenterWorld(current) };
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

        private float Heuristic(Vector3Int current, List<Vector3Int> goalCells)
        {
            float minDistance = float.MaxValue;

            foreach (var goal in goalCells)
            {
                float distance = Mathf.Abs(current.x - goal.x) + Mathf.Abs(current.y - goal.y);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }

            return minDistance;
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
            Vector3 worldPosition = _tilemap.GetCellCenterWorld(cellPosition);

            if (!_tilemap.HasTile(cellPosition))
            {
                return true;
            }

            return Physics2D.OverlapCircle(worldPosition, 0.25f, _colliderMasks) is not null;
        }
    }
}