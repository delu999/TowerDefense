using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private readonly int _width;
    private readonly int _height;
    private readonly float _tileSize;

    public Pathfinding(int width, int height, float tileSize)
    {
        _width = width;
        _height = height;
        _tileSize = tileSize;
    }

    public List<Vector2> FindPath(Vector2 start, Vector2 goal, HashSet<Vector2> occupiedCells)
    {
        start = GetGridPosition(start);
        goal = GetGridPosition(goal);
        
        PriorityQueue<Vector2> openSet = new();
        openSet.Enqueue(start, 0);

        Dictionary<Vector2, Vector2> cameFrom = new();
        Dictionary<Vector2, float> gScore = new() { [start] = 0 };
        Dictionary<Vector2, float> fScore = new() { [start] = Heuristic(start, goal) };

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (var neighbor in GetNeighbors(current))
            {
                if (occupiedCells.Contains(neighbor)) continue;

                var tentativeGScore = gScore[current] + Vector2.Distance(current, neighbor);

                if (gScore.ContainsKey(neighbor) && !(tentativeGScore < gScore[neighbor])) continue;
                
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
                openSet.Enqueue(neighbor, fScore[neighbor]);
            }
        }

        return null; // No path found
    }

    private List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
    {
        var path = new List<Vector2> { GetTransformPosition(current) };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(GetTransformPosition(current));
        }
        
        path.Reverse();
        return path;
    }

    private float Heuristic(Vector2 a, Vector2 b)
    {
        // Use Manhattan distance as the heuristic
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private IEnumerable<Vector2> GetNeighbors(Vector2 current)
    {
        Vector2[] directions = {
            new(1, 0), // right
            new(-1, 0), // left
            new(0, 1), // up
            new(0, -1) // down
        };

        foreach (var direction in directions)
        {
            var neighbor = current + direction;
            if (neighbor.x >= 0 && neighbor.x < _width && neighbor.y >= 0 && neighbor.y < _height)
            {
                yield return neighbor;
            }
        }
    }
    
    private Vector2 GetGridPosition(Vector2 t)
    {
        return new Vector2(
            Mathf.Round(t.x / _tileSize), 
            Mathf.Round(t.y / _tileSize));
    }
    
    private Vector2 GetTransformPosition(Vector2 t)
    {
        return t * _tileSize;
    }
}
