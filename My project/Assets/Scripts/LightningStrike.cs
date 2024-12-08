using System.Collections;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    public float lightningCooldown = 5f; // 每次雷擊的冷卻時間
    public GameObject lightningPrefab; // 雷擊特效預製件
    public GameObject warningCirclePrefab; // 地面傷害範圍顯示預製件
    public Transform platformCenter; // 圓形平台的中心點
    public float platformRadius = 10f; // 圓形平台的半徑
    public int minStrikes = 2; // 最小雷擊數量
    public int maxStrikes = 3; // 最大雷擊數量
    public float warningDuration = 1.5f; // 傷害範圍顯示持續時間
    public int lightningDamage = 20; // 雷擊傷害
    public float strikeRadius = 5f; // 雷擊傷害範圍半徑

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 獲取玩家
    }

    public void TriggerLightning()
    {
        StartCoroutine(GenerateLightningStrikes());
    }

    IEnumerator GenerateLightningStrikes()
    {
        // 隨機決定生成的雷擊數量
        int strikeCount = Random.Range(minStrikes, maxStrikes + 1);

        for (int i = 0; i < strikeCount; i++)
        {
            // 計算隨機位置
            Vector3 randomPosition = GetRandomPointInCircle();

            // 在隨機位置生成警告範圍
            GameObject warning = Instantiate(warningCirclePrefab, randomPosition, Quaternion.Euler(0, 0, 0));
            // 保持 Y 軸不變，僅調整 X 和 Z 軸的縮放
            Vector3 currentScale = warning.transform.localScale; // 獲取當前的縮放
            warning.transform.localScale = new Vector3(strikeRadius * 2, currentScale.y, strikeRadius * 2);


            yield return new WaitForSeconds(warningDuration);

            // 在警告範圍位置生成雷擊
            Instantiate(lightningPrefab, randomPosition, Quaternion.identity);

            // 檢查玩家是否在雷擊範圍內
            float distanceToPlayer = Vector3.Distance(player.position, randomPosition);
            if (distanceToPlayer <= strikeRadius)
            {
                player.GetComponent<PlayerHealth>()?.TakeDamage(lightningDamage);
                Debug.Log("Player hit by lightning! Took " + lightningDamage + " damage.");
            }

            // 銷毀警告範圍
            Destroy(warning);
        }
    }

    Vector3 GetRandomPointInCircle()
    {
        // 隨機取樣極坐標
        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Random.Range(0f, platformRadius);
        return new Vector3(
            platformCenter.position.x + Mathf.Cos(angle) * radius,
            platformCenter.position.y,
            platformCenter.position.z + Mathf.Sin(angle) * radius
        );
    }
}
