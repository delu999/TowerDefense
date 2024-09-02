using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "ScriptableObjects/WaveConfig", order = 1)]
public class WaveConfig : ScriptableObject
{
    public List<Wave> waves;

    [Serializable]
    public class Wave
    {
        public List<EnemySpawnInfo> enemies;
    }

    [Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public int quantity;
        public float difficulty;
    }
}