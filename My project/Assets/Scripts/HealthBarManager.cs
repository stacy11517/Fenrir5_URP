using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Transform player;                // ���a Transform
    public string enemyTag = "Enemy";       // �ĤH����
    public float detectionRadius = 20f;     // �˴��d��

    public GameObject healthBarUI;          // ��q�� UI�]�b�e�����^
    public Image healthFillImage;           // ��q������R�Ϥ�

    private EnemyController currentTarget;  // ��e�̪񪺼ĤH

    void Start()
    {
        if (healthBarUI != null)
        {
            healthBarUI.SetActive(false); // ��l�����æ�q��
        }
    }

    void Update()
    {
        // �˴��̪񪺼ĤH
        EnemyController nearestEnemy = GetNearestEnemy();
        if (nearestEnemy != currentTarget)
        {
            // �����ؼ�
            currentTarget = nearestEnemy;

            if (currentTarget != null)
            {
                healthBarUI.SetActive(true);
                UpdateHealthBar(currentTarget);
            }
            else
            {
                healthBarUI.SetActive(false); // �L�ĤH�����æ�q��
            }
        }

        // ��s��e�ؼЪ���q��
        if (currentTarget != null)
        {
            UpdateHealthBar(currentTarget);
        }
    }

    // �˴��̪񪺼ĤH
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

    // ��s��q��
    void UpdateHealthBar(EnemyController enemy)
    {
        if (enemy == null || healthFillImage == null) return;

        healthFillImage.fillAmount = (float)enemy.currentHealth / enemy.maxHealth;
    }
}
