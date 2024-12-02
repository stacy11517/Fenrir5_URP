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

    private EnemyController currentTarget;  // 當前最近的敵人
    private Camera mainCamera;              // 主攝影機

    void Start()
    {
        mainCamera = Camera.main;
        healthBarUI.SetActive(false); // 初始時隱藏血量條
    }

    void Update()
    {
        // 檢測最近的敵人
        EnemyController nearestEnemy = GetNearestEnemy();
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
    EnemyController GetNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, detectionRadius);
        float closestDistance = Mathf.Infinity;
        EnemyController nearestEnemy = null;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag(enemyTag))
            {
                EnemyController enemy = collider.GetComponent<EnemyController>();
                if (enemy != null && !enemy.isDead)
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
    void UpdateHealthBar(EnemyController enemy)
    {
        // 更新血量比例
        healthFillImage.fillAmount = (float)enemy.currentHealth / enemy.maxHealth;

        // 獲取敵人在屏幕上的位置，將其顯示在畫面頂部
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(enemy.transform.position);
        healthBarUI.transform.position = new Vector3(Screen.width / 2, Screen.height - 50, 0); // 固定在畫面正上方
    }
}
