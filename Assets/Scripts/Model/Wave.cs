using UnityEngine;

[System.Serializable]
public class Wave
{
    public int enemyID; // Array of enemy types in this wave
    public int enemyNumber; // Number of each type of enemy
    public float rate; // Spawn rate for this wave
    public float difficulty = 1; // Difficulty of the wave
}
