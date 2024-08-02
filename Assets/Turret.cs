using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float range = 3f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private LayerMask enemyMask;

    private float _fireCountdown;
    private Enemy _targetEnemy;

    private void Start()
    {
        _fireCountdown = 1f / fireRate;
    }

    private void Update() {
        try {
            if (_targetEnemy is null || Vector2.Distance(transform.position, _targetEnemy.transform.position) > range) {
                FindTarget();
            }
        }
        catch (Exception e)
        {
            _targetEnemy = null;
        }

        if (_targetEnemy is null) return;

        
        if (_targetEnemy is not null && _fireCountdown <= 0f) {
            Shoot();
            _fireCountdown = 1f / fireRate;
        }

        _fireCountdown -= Time.deltaTime;
    }

    private void FindTarget() {
        Collider2D[] colliders = new Collider2D[30];
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, range, colliders, enemyMask);
        if (size == 0)
        {
            _targetEnemy = null;
            return;
        }
        Debug.Log("Torvato!");
        
        _targetEnemy = colliders[0].GetComponent<Enemy>();
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
        
        Debug.Log("Sparo!");

        Bullet bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bulletObj.Init( transform, _targetEnemy.transform, range);
    }

    private float GetRange()
    {
        return 0f; //range * _tileSize;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
