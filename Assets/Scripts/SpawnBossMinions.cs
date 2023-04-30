using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBossMinions : MonoBehaviour
{
    [SerializeField] private GameObject[] minionPrefab;
    [SerializeField] private GameObject[] powerupPrefab;
    private float spawnRange = 9f;

    private int minionsSpawned = 0;
    private bool powerupSpawn = false;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnMinion), 2f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(powerupSpawn) {
            SpawnPowerup();
            powerupSpawn = false;
        }
    }

    void SpawnPowerup() {
        int randomPowerupSpawn = Random.Range(0, powerupPrefab.Length);
        Instantiate(powerupPrefab[randomPowerupSpawn], GenerateSpawnPosition(), powerupPrefab[randomPowerupSpawn].transform.rotation);
    }

    private Vector3 GenerateSpawnPosition() {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomSpawnPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomSpawnPos;
    }

    void SpawnMinion() {
        minionsSpawned++;
        int randomMinionSpawn = Random.Range(0, minionPrefab.Length);
        Instantiate(minionPrefab[randomMinionSpawn], GenerateSpawnPosition(), minionPrefab[randomMinionSpawn].transform.rotation);

        if(minionsSpawned%3 == 0) {
            powerupSpawn = true;
        }
    }
}
