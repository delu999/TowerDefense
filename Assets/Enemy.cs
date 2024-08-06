using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private LayerMask turretMask;

    private Pathfinding _pathfinding;
    private List<Vector2> _path;
    private Vector2 _targetPosition;
    private bool _isRecalculatingPath;
    private int _currentHealth;
    private int _pathIndex;

    private Vector2 _spawnPosition;
    
    public void Init(Vector2 spawnPosition, Vector2 targetPosition, Tilemap ground)
    {
        _currentHealth = maxHealth;
        transform.position = spawnPosition;
        _targetPosition = targetPosition;
        _spawnPosition = spawnPosition;

        _pathfinding = new Pathfinding(turretMask, ground);
        StartCoroutine(CalculatePathCoroutine());
    }

    private void Update()
    {
        if (_path == null || _pathIndex >= _path.Count) return;

        if (Vector2.Distance(_path[_pathIndex], transform.position) <= 0.1f)
        {
            _pathIndex++;

            if (_pathIndex >= _path.Count)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isRecalculatingPath) return;

        if (_path == null || _pathIndex >= _path.Count) return;

        var direction = (_path[_pathIndex] - (Vector2)transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    private IEnumerator CalculatePathCoroutine()
    {
        _isRecalculatingPath = true;
        _path = _pathfinding.FindPath(transform.position, _targetPosition);

        _pathIndex = 0;
        _isRecalculatingPath = false;

        if (_path == null)
        {
            Debug.LogWarning("Path is null, enemy might be stuck!");
        }

        Debug.Log("Path recalculated successfully.");
        yield break;
    }

    public void RecalculatePath()
    {
        if (_isRecalculatingPath) return;
        StartCoroutine(CalculatePathCoroutine());
    }
}
