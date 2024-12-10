using System.Collections;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    public GameObject healthPackPrefab; // 补包的Prefab
    public Transform[] spawnPoints;    // 补包的生成点
    public float respawnTime = 20f;    // 重新生成补包的时间

    private GameObject[] currentHealthPacks; // 当前场景中每个生成点的补包实例
    private bool[] isRespawning; // 标记每个生成点是否正在重新生成

    void Start()
    {
        // 检查是否正确设置
        if (healthPackPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("HealthPackSpawner 未正确配置：请检查Prefab和生成点！");
            return;
        }

        // 初始化补包数组和标志位数组
        currentHealthPacks = new GameObject[spawnPoints.Length];
        isRespawning = new bool[spawnPoints.Length];

        // 在每个生成点生成补包
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnHealthPackAt(i);
        }
    }

    void Update()
    {
        // 检查每个生成点是否需要重新生成补包
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (currentHealthPacks[i] == null && !isRespawning[i])
            {
                StartCoroutine(RespawnHealthPack(i));
            }
        }
    }

    // 在指定生成点生成补包
    private void SpawnHealthPackAt(int index)
    {
        if (healthPackPrefab == null || spawnPoints == null || index >= spawnPoints.Length)
        {
            Debug.LogWarning("补包Prefab或生成点未正确设置！");
            return;
        }

        Transform spawnPoint = spawnPoints[index];
        currentHealthPacks[index] = Instantiate(healthPackPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    // 等待一定时间后重新生成补包
    private IEnumerator RespawnHealthPack(int index)
    {
        isRespawning[index] = true; // 设置正在重新生成标志
        yield return new WaitForSeconds(respawnTime);

        // 检查是否已经生成补包（避免重复生成）
        if (currentHealthPacks[index] == null)
        {
            SpawnHealthPackAt(index);
        }

        isRespawning[index] = false; // 重置标志位
    }
}
