using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // �ĤH�w�s��
    public Transform[] spawnPoints; // �ĤH�ͦ��I
    public float spawnInterval = 1.5f; // �C���ͦ������j�ɶ�
    public int spawnBatchSize = 5; // �C���ͦ����ĤH�ƶq
    public int maxEnemiesPerSpawnPoint = 20; // �C�ӥͦ��I���̤j�ĤH�ƶq

    private Dictionary<Transform, int> spawnPointEnemyCounts; // �C�ӥͦ��I��e�ĤH�ƶq

    void Start()
    {
        // ��l�ƥͦ��I�p��
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
                // �p�G�ͦ��I���ĤH�ƶq���F��̤j�ƶq
                int currentEnemyCount = spawnPointEnemyCounts[spawnPoint];
                if (currentEnemyCount < maxEnemiesPerSpawnPoint)
                {
                    // ��q�ͦ��ĤH
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
            agent.enabled = true; // �T�O NavMeshAgent �ҥ�
            agent.SetDestination(GetPlayerPosition()); // �]�w�ؼЦ�m
        }

        // �q�\�ĤH���`�ƥ�
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.portalManager = FindObjectOfType<PortalManager>(); // �ʺA�]�w PortalManager
            enemyHealth.OnEnemyDeath += () => OnEnemyDeath(spawnPoint); // �q�\���`�ƥ�
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
