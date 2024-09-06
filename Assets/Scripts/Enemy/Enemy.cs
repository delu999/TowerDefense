using System.Collections;
using System.Collections.Generic;
using PathFinding;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private LayerMask turretMask;
    
        private BaseLife baseLife;
    
        [SerializeField] private int damageToBase = 1;
        [SerializeField] private float moveSpeed = 1f;
        private float moveSpeedScalingFactor = 1f;
        [SerializeField] protected float maxHealth;
        [SerializeField] protected int reward = 1;
        protected float difficulty;
        private const float RotationSpeed = 400f;

        private Pathfinding _pathfinding;
        protected List<Vector2> _path;
        protected List<Vector2> _targetPositions;
        private bool _isRecalculatingPath;
        protected float _currentHealth;
        private int _pathIndex;

        public void Init(Vector2 spawnPosition, List<Vector2> targetPositions, Tilemap ground)
        {
            _currentHealth = maxHealth;
            transform.position = spawnPosition;
            _targetPositions = targetPositions;

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

        protected virtual IEnumerator CalculatePathCoroutine()
        {
            _isRecalculatingPath = true;
            _path = _pathfinding.FindPath(transform.position, _targetPositions);

            _pathIndex = 0;
            _isRecalculatingPath = false;
            
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }

        protected virtual void OnDestroy()
        {
            EnemySpawner.Instance?.RemoveEnemy(this);
        }
    
        public void SetDifficulty(float _difficulty)
        {
            difficulty = _difficulty;
            _currentHealth *= difficulty;
        }
    }
}
