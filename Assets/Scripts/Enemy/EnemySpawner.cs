using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public List<GameObject> listEnemy;
        public int enemyCount;
        public int enemyPerWave;
        public float spawnInterval;
    }

    public List<Wave> waves;
    private int currentWaveIndex = 0;
    public float spawnTimer;
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    private void Start()
    {

    }

    private void Update()
    {
        if (currentWaveIndex < waves.Count)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= waves[currentWaveIndex].spawnInterval)
            {
                spawnTimer = 0f;
                SpawnEnemies();
                waves[currentWaveIndex].enemyPerWave++;
                if (waves[currentWaveIndex].enemyPerWave > waves[currentWaveIndex].enemyCount)
                {
                    waves[currentWaveIndex].enemyPerWave = 0; // Reset số lượng đã spawn trong wave này
                    if (waves[currentWaveIndex].spawnInterval > 1f)
                    {
                        // Càng về sau khi spawn càng nhanh hơn
                        waves[currentWaveIndex].spawnInterval *= 0.9f;
                    }
                    currentWaveIndex++;
                }
            }
        }
        else
        {
            currentWaveIndex = 0; // Reset về wave đầu
        }
    }

    private void SpawnEnemies()
    {
        // Random chọn một enemy từ danh sách trong wave hiện tại
        GameObject enemyPrefab = waves[currentWaveIndex].listEnemy[Random.Range(0, waves[currentWaveIndex].listEnemy.Count)];
        GameObject enemy = MyPoolManager.Instance.GetFromPool(enemyPrefab, null);
        if (enemy != null)
        {
            enemy.transform.position = RandomSpawnPoint();
            enemy.SetActive(true);
        }
    }

    private Vector2 RandomSpawnPoint()
    {
        Vector2 spawnPoint;

        if (Random.Range(0f, 1f) < 0.5f)
        {
            spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
            if (Random.Range(0f, 1f) < 0.5f)
            {
                spawnPoint.y = minPos.position.y; // Spawn ở dưới
            }
            else
            {
                spawnPoint.y = maxPos.position.y; // Spawn ở trên
            }
        }
        else
        {
            spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);
            if (Random.Range(0f, 1f) < 0.5f)
            {
                spawnPoint.x = minPos.position.x; // Spawn bên trái
            }
            else
            {
                spawnPoint.x = maxPos.position.x; // Spawn bên phải
            }
        }


        return spawnPoint;
    }
}
