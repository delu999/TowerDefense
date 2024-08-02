using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int bulletDamage = 1;
    
    private Transform _origin;
    private Transform _target;
    private float _range;

    private void Update()
    {
        if (Vector2.Distance(transform.position, _origin.position) > _range)
        {
            Destroy(gameObject);
        }
    }

    public void Init(Transform origin, Transform target, float range) {
        _target = target;
        _range = range;
        _origin = origin;
    }

    private void FixedUpdate() {
        if (!_target) return;

        Vector2 direction = (_target.position - transform.position).normalized;

        rb.velocity = direction * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
    
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}