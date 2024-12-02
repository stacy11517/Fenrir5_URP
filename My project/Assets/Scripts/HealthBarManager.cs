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
    private Camera mainCamera;              // �D��v��

    void Start()
    {
        mainCamera = Camera.main;
        healthBarUI.SetActive(false); // ��l�����æ�q��
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
        // ��s��q���
        healthFillImage.fillAmount = (float)enemy.currentHealth / enemy.maxHealth;

        // ����ĤH�b�̹��W����m�A�N����ܦb�e������
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(enemy.transform.position);
        healthBarUI.transform.position = new Vector3(Screen.width / 2, Screen.height - 50, 0); // �T�w�b�e�����W��
    }
}
