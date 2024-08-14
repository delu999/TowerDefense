using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private LayerMask turretMasks;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Transform> basePoints;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxWaveCycles = 3;
    private readonly List<Enemy> _enemies = new ();
    private Pathfinding _pathfinding;

    public Button startWaveButton;
    public TextMeshProUGUI waveUI;
    private int _currentWave;
    
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
        startWaveButton.onClick.AddListener(StartSpawning);
    }

    private void OnGUI()
    {
        waveUI.text = _currentWave.ToString();
    }

    public void StartSpawning()
    {
        _currentWave = 0;
        StartCoroutine(SpawnEnemies());
    }
    
    private IEnumerator SpawnEnemies()
    {
        startWaveButton.interactable = false;
        float _currentDifficulty = 1f;
        for (int i = 0; i < prefabs.Count * maxWaveCycles; i++)
        {
            _currentWave++;
            bool isBoss = i % prefabs.Count == prefabs.Count - 1;
            StartCoroutine(SpawnWave(prefabs[i%prefabs.Count], isBoss ? 1 : 10, _currentDifficulty));
            if (isBoss) _currentDifficulty *= 2;
            yield return new WaitForSeconds(10f);
        }
        startWaveButton.interactable = true;
    }
    
    private void SpawnEnemy(GameObject enemyToSpawn, float difficulty) {
        int randomSpawnPointID = Random.Range(0, spawnPoints.Count);
        int randomBasePointID = Random.Range(0, basePoints.Count);

        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPoints[randomSpawnPointID].position, Quaternion.identity);
        Enemy enemy = spawnedEnemy.GetComponent<Enemy>();
        enemy.SetDifficulty(difficulty);
        _enemies.Add(enemy);
        enemy.Init(spawnPoints[randomSpawnPointID].position, basePoints[randomBasePointID].position, tilemap);
    }
    
    private IEnumerator SpawnWave(GameObject enemy, int quantity, float difficulty)
    {
        for (int j = 0; j < quantity; j++)
        {
            SpawnEnemy(enemy, difficulty);
            yield return new WaitForSeconds(0.5f);
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
