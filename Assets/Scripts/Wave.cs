using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject[] enemyPrefabs; // Array of enemy types in this wave
    public int[] counts; // Number of each type of enemy
    public float rate; // Spawn rate for this wave

    public void Initialize()
    {
        if (enemyPrefabs.Length != counts.Length)
        {
            Debug.LogError("Mismatch between enemyPrefabs and counts in wave configuration.");
        }
    }
}
