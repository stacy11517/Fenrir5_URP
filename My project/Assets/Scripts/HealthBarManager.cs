using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Transform player;                // 玩家 Transform
    public string enemyTag = "Enemy";       // 敵人標籤
    public float detectionRadius = 20f;     // 檢測範圍

    public GameObject healthBarUI;          // 血量條 UI（在畫布中）
    public Image healthFillImage;           // 血量條的填充圖片

    private EnemyHealth currentTarget;      // 當前最近的敵人

    void Start()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false); // 初始時隱藏血量條
        }
    }

    void Update()
    {
        // 檢測最近的敵人
        EnemyHealth nearestEnemy = GetNearestEnemy();
        if (nearestEnemy != currentTarget)
        {
            // 切換目標
            currentTarget = nearestEnemy;

            if (currentTarget != null)
            {
                healthBarUI.SetActive(true);
                UpdateHealthBar(currentTarget);
            }
            else
            {
                healthBarUI.SetActive(false); // 無敵人時隱藏血量條
            }
        }

        // 更新當前目標的血量條
        if (currentTarget != null)
        {
            UpdateHealthBar(currentTarget);
        }
    }

    // 檢測最近的敵人
    EnemyHealth GetNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, detectionRadius);
        float closestDistance = Mathf.Infinity;
        EnemyHealth nearestEnemy = null;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(enemyTag))
            {
                EnemyHealth enemy = collider.GetComponent<EnemyHealth>();
                if (enemy != null && !enemy.IsDead())
                {
                    float distance = Vector3.Distance(player.position, enemy.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
            }
        }

        return nearestEnemy;
    }

    // 更新血量條
    void UpdateHealthBar(EnemyHealth enemy)
    {
        if (enemy == null || healthFillImage == null) return;

        healthFillImage.fillAmount = (float)enemy.currentHealth / enemy.maxHealth;
    }
}
