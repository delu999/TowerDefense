using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private LayerMask turretMasks;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Transform> basePoints;
    [SerializeField] private List<Wave> waves;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float spawnInterval = 2f;
    private readonly List<Enemy> _enemies = new ();
    private Pathfinding _pathfinding;

    public static EnemySpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _pathfinding = new Pathfinding(turretMasks, tilemap);
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
    }
    
    private IEnumerator SpawnEnemies()
    {
        foreach (var wave in waves)
        {
            StartCoroutine(SpawnWave(wave));
            yield return new WaitForSeconds(20f);
        }
    }
    
    private void SpawnEnemy(GameObject enemyToSpawn) {
        int randomSpawnPointID = Random.Range(0, spawnPoints.Count);
        int randomBasePointID = Random.Range(0, basePoints.Count);

        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPoints[randomSpawnPointID].position, Quaternion.identity);
        Enemy enemy = spawnedEnemy.GetComponent<Enemy>();
        _enemies.Add(enemy);
        enemy.Init(spawnPoints[randomSpawnPointID].position, basePoints[randomBasePointID].position, tilemap);
    }
    
    private IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.enemyPrefabs.Length; i++)
        {
            for (int j = 0; j < wave.counts[i]; j++)
            {
                SpawnEnemy(wave.enemyPrefabs[i]);
                yield return new WaitForSeconds(1f / wave.rate);
            }
        }
    }
    
    public void RecalculatePaths()
    {
        foreach (var enemy in _enemies)
        {
            enemy?.RecalculatePath();
        }
    }
    
    public bool IsPathAvailable()
    {
        return _pathfinding.FindPath(spawnPoints[0].position, basePoints[0].position) is not null;
    }
    
    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }
    
    public void Restore()
    {
        Instance = null;
    }
}
