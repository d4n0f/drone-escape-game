using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] GameObject birdPrefab;

    public float spawnMax = 70;
    public float spawnMin = 40;
    public float startDelay = 1;
    public float spawnInterval = 1f;
    public float maxEnemy = 30;
    int enemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        InvokeRepeating(nameof(SpawnEnemy), startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        if (gameManager.isGameOver)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }

        if (enemyCount >= maxEnemy) return;

        float spawnPosX;
        float spawnPosZ;
        do
        {
            spawnPosX = Random.Range(-spawnMax, spawnMax);
            spawnPosZ = Random.Range(-spawnMax, spawnMax);
        }
        while (Mathf.Abs(spawnPosX) < spawnMin && Mathf.Abs(spawnPosZ) < spawnMin);

        Vector3 spawnPos = new Vector3(spawnPosX, 5, spawnPosZ);

        Instantiate(birdPrefab, spawnPos, birdPrefab.transform.rotation);
        enemyCount++;
    }

    public void EnemyDied()
    {
        enemyCount--;
    }
}
