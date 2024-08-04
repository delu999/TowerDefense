using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] public float range = 3f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 400f;
    [SerializeField] private int damage = 1;
    [SerializeField] public int cost = 50;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Transform turretRotationPoint;

    private float _fireCountdown;
    private Enemy _targetEnemy;

    private void Start()
    {
        _fireCountdown = 1f / fireRate;
    }

    private void Update() {
        try {
            if (_targetEnemy is null || Vector2.Distance(transform.position, _targetEnemy.transform.position) > GetRange()) {
                FindTarget();
            }
        }
        catch (Exception e)
        {
            _targetEnemy = null;
        }

        if (_targetEnemy is null) return;

        RotateTowardsTarget();
        
        if (_targetEnemy is not null && _fireCountdown <= 0f) {
            Shoot();
            _fireCountdown = 1f / fireRate;
        }

        _fireCountdown -= Time.deltaTime;
    }
    
    private void FindTarget() {
        Collider2D[] colliders = new Collider2D[30];
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, GetRange(), colliders, enemyMask);
        if (size == 0)
        {
            _targetEnemy = null;
            return;
        }
        
        _targetEnemy = colliders[0].GetComponent<Enemy>();
    }

    private void RotateTowardsTarget()
    {
        if (_targetEnemy is null) return;
        float angle = Mathf.Atan2(_targetEnemy.transform.position.y - transform.position.y, _targetEnemy.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        if (_targetEnemy is null) return;

        // Verifica se il targetEnemy è stato distrutto
        if (_targetEnemy.gameObject == null)
        {
            _targetEnemy = null;
            return;
        }

        Bullet bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        bulletObj.Init( firingPoint, _targetEnemy.transform, GetRange(), damage);
    }

    private float GetRange()
    {
        return range; //range * _tileSize;
    }
}
