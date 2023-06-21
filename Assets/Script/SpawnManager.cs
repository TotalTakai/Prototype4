using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private readonly float spawnBoundary = 9;
    private readonly int bossSpawnWave = 5;
    private int enemyCount;
    private int waveNumber = 0;
    private float bossTimer = 0;
    private bool isBossSpawned = false;
    

    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs;
    public GameObject enemyBoss;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            if (waveNumber == bossSpawnWave)
            {
                SpawnEnemyBoss();
                isBossSpawned = true;
            }
            else
            {
                SpawnEnemyWave(waveNumber);
                SpawnPowerUp();
            }
        }
        if (isBossSpawned)
        {
            BossSpawns();
        }
    }

    void BossSpawns()
    {
        if (waveNumber == bossSpawnWave)
        {
            bossTimer += Time.deltaTime;
            if (bossTimer >= 8)
            {
                SpawnEnemyWave(3);
                SpawnPowerUp();
                bossTimer = 0;
            }
        }
        else isBossSpawned = false;
    }
    //Spawns a power up
    private void SpawnPowerUp()
    {
        int randomPowerup = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);
    }

    //Spawns enemies according to the number of wave
    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for(int i=0; i<enemiesToSpawn; i++)
        {
            int randomEnemy = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemy], GenerateSpawnPosition(), enemyPrefabs[randomEnemy].transform.rotation);
        }
    }

    private void SpawnEnemyBoss()
    {
        float spawnPosX = Random.Range(-spawnBoundary, spawnBoundary);
        float spawnPosZ = Random.Range(-spawnBoundary, spawnBoundary);
        Instantiate(enemyBoss, new Vector3(spawnPosX, 0.75f, spawnPosZ), enemyBoss.transform.rotation);
    }

    // Generates a random spawn position
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnBoundary, spawnBoundary);
        float spawnPosZ = Random.Range(-spawnBoundary, spawnBoundary);
        Vector3 spawnPosition = new Vector3(spawnPosX, 0 , spawnPosZ);
        return spawnPosition;
    }
}
