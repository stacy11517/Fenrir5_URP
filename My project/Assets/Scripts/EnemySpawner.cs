using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 敵人預製件
    public Transform[] spawnPoints; // 敵人生成點
    public float spawnInterval = 1.5f; // 每次生成的間隔時間
    public int spawnBatchSize = 5; // 每次生成的敵人數量
    public int maxEnemiesPerSpawnPoint = 20; // 每個生成點的最大敵人數量

    private Dictionary<Transform, int> spawnPointEnemyCounts; // 每個生成點當前敵人數量

    void Start()
    {
        // 初始化生成點計數
        spawnPointEnemyCounts = new Dictionary<Transform, int>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            spawnPointEnemyCounts[spawnPoint] = 0;
        }

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                // 如果生成點的敵人數量未達到最大數量
                int currentEnemyCount = spawnPointEnemyCounts[spawnPoint];
                if (currentEnemyCount < maxEnemiesPerSpawnPoint)
                {
                    // 批量生成敵人
                    for (int i = 0; i < spawnBatchSize && currentEnemyCount < maxEnemiesPerSpawnPoint; i++)
                    {
                        SpawnEnemy(spawnPoint);
                        currentEnemyCount++;
                    }
                    spawnPointEnemyCounts[spawnPoint] = currentEnemyCount;
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy(Transform spawnPoint)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.enabled = true; // 確保 NavMeshAgent 啟用
            agent.SetDestination(GetPlayerPosition()); // 設定目標位置
        }

        // 訂閱敵人死亡事件
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.portalManager = FindObjectOfType<PortalManager>(); // 動態設定 PortalManager
            enemyHealth.OnEnemyDeath += () => OnEnemyDeath(spawnPoint); // 訂閱死亡事件
        }
    }

    void OnEnemyDeath(Transform spawnPoint)
    {
        if (spawnPointEnemyCounts.ContainsKey(spawnPoint))
        {
            spawnPointEnemyCounts[spawnPoint]--;
        }
    }

    Vector3 GetPlayerPosition()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            return player.transform.position;
        }
        return Vector3.zero;
    }
}
