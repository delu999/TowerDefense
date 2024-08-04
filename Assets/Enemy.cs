using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int rewardAmount = 10;
    [SerializeField] public int damageToBase = 1;
    private BaseLife baseLife;
    
    private GridManager _gridManager;
    private Pathfinding _pathfinding;
    private List<Vector2> _path;
    private Vector2 _targetPosition;
    private bool _isRecalculatingPath;
    private int _currentHealth;
    private int _pathIndex;
    
    public void Init(HashSet<Vector2> occupiedCells, GridManager gridManager) {
        _gridManager = gridManager;
        _currentHealth = maxHealth;
        
        //target position: X => last col, Y => current row
        _targetPosition = transform.position;
        _targetPosition.x = (_gridManager.numHorizontalTiles - 1) * _gridManager.tileSize;
        
        _pathfinding = new Pathfinding(_gridManager.numHorizontalTiles, _gridManager.numVerticalTiles, _gridManager.tileSize);
        
        StartCoroutine(CalculatePathCoroutine(occupiedCells));
    }
    private void Update()
    {
        if (_path == null || _pathIndex >= _path.Count) return;
        
        if (!(Vector2.Distance(_path[_pathIndex], transform.position) <= 0.1f)) return;

        _pathIndex++; 
        
        if (_pathIndex < _path.Count) return;

        // The enemy has reached his target
        BaseLife.main.DecreaseLife(damageToBase);
        _gridManager.RemoveEnemy(this);
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCurrency(rewardAmount);
        }
    }

    private void FixedUpdate() {
        if (_isRecalculatingPath) return;

        if (_path is null || _pathIndex >= _path.Count) return;
        
        var direction = (_path[_pathIndex] - (Vector2)transform.position).normalized;
        RotateTowardsTarget();
        rb.velocity = direction * moveSpeed;
    }

    private IEnumerator CalculatePathCoroutine(HashSet<Vector2> occupiedCells) {
        _isRecalculatingPath = true;
        _path = _pathfinding.FindPath(transform.position, _targetPosition, occupiedCells);

        _pathIndex = 1;
        _isRecalculatingPath = false;

        if (_path is null) {
            Debug.LogWarning("Path is null, enemy might be stuck!");
        }

        Debug.Log("Path recalculated successfully.");
        yield break;
    }
    
    public void RecalculatePath(HashSet<Vector2> occupiedCells)
    {
        if (_isRecalculatingPath) return;
        
        StartCoroutine(CalculatePathCoroutine(occupiedCells));
    }
    
    public void TakeDamage(int damage) {
        _currentHealth -= damage;
        if (_currentHealth > 0) return;
        
        _gridManager.RemoveEnemy(this);
        Destroy(gameObject);
    }
    
    private void RotateTowardsTarget()
    {
        if (_path is null) return;
        float angle = Mathf.Atan2(_path[_pathIndex].y - transform.position.y, _path[_pathIndex].x - transform.position.x) * Mathf.Rad2Deg - 90f;
        
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.rotation = targetRotation;
    }
}
