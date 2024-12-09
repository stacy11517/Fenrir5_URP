using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    public GameObject healthPackPrefab; // 补包的Prefab
    public Transform[] spawnPoints;    // 补包的生成点
    public float respawnTime = 20f;    // 重新生成补包的时间

    private GameObject[] currentHealthPacks; // 当前场景中每个生成点的补包实例

    void Start()
    {
        // 初始化补包数组
        currentHealthPacks = new GameObject[spawnPoints.Length];
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
            if (currentHealthPacks[i] == null)
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
    private System.Collections.IEnumerator RespawnHealthPack(int index)
    {
        yield return new WaitForSeconds(respawnTime);

        // 检查是否已经生成补包（避免重复生成）
        if (currentHealthPacks[index] == null)
        {
            SpawnHealthPackAt(index);
        }
    }
}
