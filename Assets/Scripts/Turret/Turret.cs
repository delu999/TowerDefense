using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Turret
{
    public class Turret : DefensiveStructure
    {
        [SerializeField] protected Bullet.Bullet bulletPrefab;
        [SerializeField] protected LayerMask enemyMask;
        [SerializeField] protected Transform firingPoint;
        [SerializeField] private Transform turretRotationPoint;
        [SerializeField] private GameObject rangePrefab;
    
        [SerializeField] private new string name;
        [SerializeField] protected float Range;
        [SerializeField] protected float FireRate;
        [SerializeField] protected float Damage;
        private const float RotationSpeed = 400f;
        protected float FireCountdown;
        protected Enemy.Enemy TargetEnemy;
        private GameObject _rangeGameObject;
        
        private void Start()
        {
            //Init();
            FireCountdown = 0.5f;
        }
   
        private void Update() {
            try {
                if (TargetEnemy is null || Vector2.Distance(transform.position, TargetEnemy.transform.position) > GetRange()) {
                    FindTarget();
                }
            }
            catch (Exception)
            {
                TargetEnemy = null;
            }
   
            if (TargetEnemy is null) return;
   
            RotateTowardsTarget();
        
            if (TargetEnemy is not null && FireCountdown <= 0f) {
                Shoot();
                FireCountdown = 1f / FireRate;
            }
   
            FireCountdown -= Time.deltaTime;
        }

        private void OnMouseDown()
        {
            if (!removeUI.activeSelf)
            {
                removeUI.SetActive(true);
                if(rangePrefab)
                {
                    _rangeGameObject = Instantiate(rangePrefab, transform.position, Quaternion.identity);
                    _rangeGameObject.transform.localScale = new Vector3(GetRange() * 2, GetRange() * 2, 1f);
                }
                return;
            }
    
            Vector3Int cellPosition = PlayerInput.Instance.ground.WorldToCell(transform.position);
            PlayerInput.Instance.ground.SetColliderType(cellPosition, UnityEngine.Tilemaps.Tile.ColliderType.Sprite);

            Destroy(_rangeGameObject);
            Destroy(gameObject);
            EnemySpawner.Instance.RecalculatePaths();
        }

        private void OnMouseExit()
        {
            if(removeUI.activeSelf) removeUI.SetActive(false);
            if(_rangeGameObject)
            {
                Destroy(_rangeGameObject);
                _rangeGameObject = null;
            }
        }

        private void FindTarget() {
            Collider2D[] colliders = new Collider2D[30];
            int size = Physics2D.OverlapCircleNonAlloc(transform.position, Range, colliders, enemyMask);
            
            if (size == 0)
            {
                TargetEnemy = null;
                return;
            }
        
            TargetEnemy = colliders[0].GetComponent<Enemy.Enemy>();
        }
   
        private void RotateTowardsTarget()
        {
            if (TargetEnemy is null) return;
            float angle = Mathf.Atan2(TargetEnemy.transform.position.y - transform.position.y, TargetEnemy.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
   
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
   
        protected virtual void Shoot()
        {
            if (TargetEnemy is null) return;
   
            // Verifica se il targetEnemy è stato distrutto
            if (TargetEnemy.gameObject == null)
            {
                TargetEnemy = null;
                return;
            }
   
            Bullet.Bullet bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            bulletObj.Init( firingPoint, TargetEnemy.transform, GetRange(), Damage);
        }
   
        public virtual float GetRange()
        {
            return Range;
        }
    }
}
