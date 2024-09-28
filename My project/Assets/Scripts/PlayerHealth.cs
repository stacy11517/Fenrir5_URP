using UnityEngine;
using UnityEngine.UI;          // �Ω� UI �ާ@
using TMPro;                 // �Ω� TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;          // ���a�̤j�ͩR��
    public int currentHealth;             // ���a��e�ͩR��
    public int healthPackCount = 0;       // �ɦ�]���ƶq
    public Image healthBar;                // ��� UI
    public TMP_Text healthPackText;        // ��q�]�ƶq�� TextMeshPro ����

    void Start()
    {
        // ��l�ƪ��a�ͩR��
        currentHealth = maxHealth;
        UpdateHealthBar();              // ��l�Ʀ��
        UpdateHealthPackText();         // ��l�Ƹɦ�]�ƶq�奻
    }

    // ��_�ͩR�Ȫ���k
    public void Heal(int amount)
    {
        // ��_�ͩR�ȡA�����W�L�̤j�ͩR��
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player healed! Current health: " + currentHealth);
        UpdateHealthBar();              // ��s���
    }

    // ��I��ɦ�]��Ĳ�o
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPack"))
        {
            // �߰_�ɦ�]�A�ƶq�W�[
            healthPackCount++;
            Debug.Log("Health pack picked up! Total packs: " + healthPackCount);
            UpdateHealthPackText();       // ��s�ɦ�]�ƶq�奻

            // �P���ɦ�]����
            Destroy(other.gameObject);
        }
    }

    // �ϥθɦ�]
    void Update()
    {
        // ���U X ��Өϥθɦ�]
        if (Input.GetKeyDown(KeyCode.X) && healthPackCount > 0)
        {
            UseHealthPack();
        }
    }

    // �ϥθɦ�]��_�ͩR
    void UseHealthPack()
    {
        if (currentHealth < maxHealth)
        {
            Heal(10);                    // ��_10�I�ͩR��
            healthPackCount--;           // �ɦ�]�ƶq���
            Debug.Log("Health pack used! Remaining packs: " + healthPackCount);
            UpdateHealthPackText();      // ��s�ɦ�]�ƶq�奻
        }
        else
        {
            Debug.Log("Health is full, cannot use health pack.");
        }
    }

    // ��s������
    void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthBar.fillAmount = healthPercent;  // �վ���������
    }

    // ��s�ɦ�]�ƶq�奻���
    void UpdateHealthPackText()
    {
        healthPackText.text = healthPackCount > 0 ? healthPackCount.ToString() : "";  // ��ƶq�j�� 0 ����ܼƦr�A�_�h�����
    }
}
