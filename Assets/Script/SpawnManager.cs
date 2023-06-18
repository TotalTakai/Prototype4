using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private readonly float spawnBoundary = 9;

    public GameObject enemyPrefub;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(enemyPrefub, GenerateSpawnPosition(), enemyPrefub.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Generates a random spawn position
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnBoundary, spawnBoundary);
        float spawnPosZ = Random.Range(-spawnBoundary, spawnBoundary);
        Vector3 spawnPosition = new Vector3(spawnPosX, 0, spawnPosZ);
        return spawnPosition;
    }
}
