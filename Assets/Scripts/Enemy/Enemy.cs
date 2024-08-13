using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask turretMask;
    
    private BaseLife baseLife;
    
    private int damageToBase = 1;
    private float moveSpeed = 1f;
    private float moveSpeedScalingFactor = 1f;
    protected float maxHealth = 20;
    protected int reward = 2;
    protected float difficulty;

    private Pathfinding _pathfinding;
    private List<Vector2> _path;
    private Vector2 _targetPosition;
    private bool _isRecalculatingPath;
    private float _currentHealth;
    private int _pathIndex;

    private Vector2 _spawnPosition;

    public virtual void Init(Vector2 spawnPosition, Vector2 targetPosition, Tilemap ground)
    {
        _currentHealth = maxHealth;
        transform.position = spawnPosition;
        _targetPosition = targetPosition;
        _spawnPosition = spawnPosition;

        _pathfinding = new Pathfinding(turretMask, ground);
        StartCoroutine(CalculatePathCoroutine());
    }

    protected virtual void Update()
    {
        if (_path == null || _pathIndex >= _path.Count) return;

        if (Vector2.Distance(_path[_pathIndex], transform.position) <= 0.1f)
        {
            _pathIndex++;

            if (_pathIndex >= _path.Count)
            {
                // The enemy has reached its target
                BaseLife.Instance.DecreaseLife(damageToBase);
                Destroy(gameObject);
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (_isRecalculatingPath) return;

        if (_path == null || _pathIndex >= _path.Count)
        {
            rb.velocity = new Vector2(0f, 0f);
            return;
        }

        var direction = (_path[_pathIndex] - (Vector2)transform.position).normalized;
        RotateTowardsTarget();
        rb.velocity = direction * GetSpeed(); // Use method to get speed
    }

    public void ChangeSpeedFactor(float factor = 1f)
    {
        moveSpeedScalingFactor = factor;
    }
    
    protected virtual float GetSpeed()
    {
        return moveSpeed * moveSpeedScalingFactor;
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

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth > 0) return;

        CurrencyManager.Instance.AddCurrency(reward);
        Destroy(gameObject);
    }

    private void RotateTowardsTarget()
    {
        if (_path is null) return;
        float angle = Mathf.Atan2(_path[_pathIndex].y - transform.position.y, _path[_pathIndex].x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation;
    }

    protected virtual void OnDestroy()
    {
        EnemySpawner.Instance.RemoveEnemy(this);
    }
    
    public void SetDifficulty(float _difficulty)
    {
        difficulty = _difficulty;
        _currentHealth *= difficulty;
        reward = Mathf.RoundToInt(reward / difficulty);
        if (reward < 1) reward = 1;
    }
}
