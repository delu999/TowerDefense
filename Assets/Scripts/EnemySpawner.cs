using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private LayerMask turretMasks;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Transform> basePoints;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float spawnInterval = 30f;
    [SerializeField] private int maxWaveCycles = 3;
    private readonly List<Enemy> _enemies = new ();
    private Pathfinding _pathfinding;
    private float countdownUntilgameEnd = 2f;

    public Button startWaveButton;
    public int CurrentWave { get; private set; }
    
    public static EnemySpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            CurrentWave = 0;
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _pathfinding = new Pathfinding(turretMasks, tilemap);
        startWaveButton.onClick.AddListener(StartSpawning);
    }

    private void Update()
    {
        if (countdownUntilgameEnd <= 0f)
        {
            Time.timeScale = 0;

            BaseLife.Instance?.Restore();

            SceneManager.LoadScene("VictoryScene");
        }
        if (CurrentWave >= GetTotalWaves() && _enemies.Count == 0)
        {
            countdownUntilgameEnd -= Time.deltaTime;
        }
    }

    public void StartSpawning()
    {
        CurrentWave = 0;
        StartCoroutine(SpawnEnemies());
    }
    
    private IEnumerator SpawnEnemies()
    {
        startWaveButton.interactable = false;
        float _currentDifficulty = 1f;
        for (int i = 0; i < GetTotalWaves(); i++)
        {
            CurrentWave++;
            bool isBoss = i % prefabs.Count == prefabs.Count - 1;
            StartCoroutine(SpawnWave(prefabs[i%prefabs.Count], isBoss ? 1 : 10, _currentDifficulty));
            if (isBoss) _currentDifficulty *= 2;
            yield return new WaitForSeconds(spawnInterval);
        }
        // startWaveButton.interactable = true;
    }

    private IEnumerator SpawnWave(GameObject enemy, int quantity, float difficulty)
    {
        for (int j = 0; j < quantity; j++)
        {
            SpawnEnemy(enemy, difficulty);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private void SpawnEnemy(GameObject enemyToSpawn, float difficulty) {
        int randomSpawnPointID = Random.Range(0, spawnPoints.Count);

        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPoints[randomSpawnPointID].position, Quaternion.identity);
        Enemy enemy = spawnedEnemy.GetComponent<Enemy>();
        enemy.SetDifficulty(difficulty);
        _enemies.Add(enemy);
        enemy.Init(spawnPoints[randomSpawnPointID].position, basePoints[randomSpawnPointID].position, tilemap);
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

    private int GetTotalWaves()
    {
        return prefabs.Count * maxWaveCycles;
    }
    
    public void Restore()
    {
        Instance = null;
    }
}
