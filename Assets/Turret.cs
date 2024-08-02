using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float range = 3f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject bulletPrefab;

    private float fireCountdown = 0f;
    private Enemy targetEnemy;

    private void Update() {
        if (targetEnemy is null || Vector2.Distance(transform.position, targetEnemy.transform.position) > range) {
            FindTarget();
        }
        
        if (targetEnemy != null && fireCountdown <= 0f) {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void FindTarget() {
        Collider2D[] colliders = new Collider2D[30];
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, range, colliders);
        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;

        for (int i = 0; i < size; i++) {
            Collider2D collider = colliders[i];
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && enemy.gameObject != null) {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance) {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= range) {
            targetEnemy = nearestEnemy;
        } else {
            targetEnemy = null;
        }
    }



    private void Shoot()
    {
        if (targetEnemy == null)
        {
            return;
        }

        // Verifica se il targetEnemy è stato distrutto
        if (targetEnemy.gameObject == null)
        {
            targetEnemy = null;
            return;
        }

        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(targetEnemy.transform);
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
