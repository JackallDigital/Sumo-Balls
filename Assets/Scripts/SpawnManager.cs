using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private GameObject[] bossPrefab;
    [SerializeField] private GameObject[] powerupPrefab;

    private float spawnRange = 9f;
    private int enemyCount;
    private int enemyMax = 1;
    private int enemyWave = 0;
    //public int playerCount;

    // Start is called before the first frame update
    void Awake()
    {
        SpawnEnemyWave(enemyMax);
        //SpawnPlayer();
        SpawnPowerup();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<EnemyController>().Length;
        //playerCount = FindObjectsOfType<PlayerController>().Length;

        if (enemyCount == 0)
        {
            enemyWave++;
            if (enemyWave % 3 == 0) {
                int spawnBoss = Random.Range(0, bossPrefab.Length);
                Instantiate(bossPrefab[spawnBoss], GenerateSpawnPosition(), enemyPrefab[spawnBoss].transform.rotation);
            }
            else {
                SpawnEnemyWave(++enemyMax);
            }
            SpawnPowerup();
        }
        //if(playerCount == 0){
        //    SpawnPlayer();
        //}
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for(int i = 0; i < enemiesToSpawn; i++)
        {
            int randomEnemySpawn = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[randomEnemySpawn], GenerateSpawnPosition(), enemyPrefab[randomEnemySpawn].transform.rotation);
        }
    }

    //void SpawnPlayer() {
    //    Instantiate(playerPrefab, GenerateSpawnPosition(), playerPrefab.transform.rotation);
    //}

    void SpawnPowerup()
    {
        int randomPowerupSpawn = Random.Range(0, powerupPrefab.Length);
        Instantiate(powerupPrefab[randomPowerupSpawn], GenerateSpawnPosition(), powerupPrefab[randomPowerupSpawn].transform.rotation);
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomSpawnPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomSpawnPos;
    }
}