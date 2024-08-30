using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveConfig waveConfig;
    [SerializeField] private LayerMask turretMasks;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Transform> basePoints;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float spawnInterval = 20f;
    private readonly List<Enemy.Enemy> _enemies = new ();
    private Pathfinding _pathfinding;
    private float countdownUntilgameEnd = 2f;
    private bool isSpawning = false;

    public Button startWaveButton;
    public int CurrentWave { get; private set; }
    public float CountdownUntilNextWave { get; private set; }

    public static EnemySpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            CurrentWave = 0;
            Instance = this;
        }
    }

    private void Start()
    {
        CountdownUntilNextWave = 0f;
        _pathfinding = new Pathfinding(turretMasks, tilemap);
        startWaveButton.onClick.AddListener(StartSpawning);
    }

    private void Update()
    {
        if (countdownUntilgameEnd <= 0f)
        {
            isSpawning = false;
            Time.timeScale = 0;
            BaseLife.Instance?.Restore();
            SceneManager.LoadScene("VictoryScene");
            return;
        }

        if (CurrentWave >= waveConfig.waves.Count && _enemies.Count == 0)
        {
            countdownUntilgameEnd -= Time.deltaTime;
        }

        // Spawn Waves
        if (!isSpawning) return;
        if (CountdownUntilNextWave > 0) {
            CountdownUntilNextWave -= Time.deltaTime;
            return;
        }

        // Wave finished, but there are more enemies spawned al alternative ways
        if (CurrentWave >= waveConfig.waves.Count) return;
        var wave = waveConfig.waves[CurrentWave];
        CurrentWave++;
        foreach (var enemyInfo in wave.enemies)
        {
            StartCoroutine(SpawnWave(enemyInfo));
        }
        CountdownUntilNextWave = spawnInterval;
    }

    public void StartSpawning()
    {
        CurrentWave = 0;
        startWaveButton.gameObject.SetActive(false);
        isSpawning = true;
    }

    public void SpawnWaveFromSinglePoint(WaveConfig.Wave wave, Vector3 spawnPoint)
    {
        foreach (var enemyInfo in wave.enemies)
        {
            StartCoroutine(SpawnWave(enemyInfo, spawnPoint));
        }
    }

    private IEnumerator SpawnWave(WaveConfig.EnemySpawnInfo enemyInfo, Vector3? spawnPoint = null)
    {
        for (int i = 0; i < enemyInfo.quantity; i++)
        {
            SpawnEnemy(enemyInfo.enemyPrefab, enemyInfo.difficulty, spawnPoint);
            yield return new WaitForSeconds(0.05f); // Delay between each enemy spawn within the wave
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab, float difficulty, Vector3? position = null)
    {
        int randomSpawnPointID = Random.Range(0, spawnPoints.Count);
        GameObject spawnedEnemy = Instantiate(enemyPrefab, position?? spawnPoints[randomSpawnPointID].position, Quaternion.identity);
        Enemy.Enemy enemy = spawnedEnemy.GetComponent<Enemy.Enemy>();
        enemy.SetDifficulty(difficulty);
        _enemies.Add(enemy);
        enemy.Init(position?? spawnPoints[randomSpawnPointID].position, basePoints[randomSpawnPointID].position, tilemap);
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
        return _pathfinding.FindPath(spawnPoints[0].position, basePoints[0].position) != null;
    }

    public void RemoveEnemy(Enemy.Enemy enemy)
    {
        _enemies.Remove(enemy);
    }

    public void Restore()
    {
        Instance = null;
    }
}
