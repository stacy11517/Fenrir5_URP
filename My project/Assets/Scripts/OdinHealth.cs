using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OdinHealth : MonoBehaviour
{
    public int maxHealth = 500;          // ���B���̤j��q
    public int currentHealth;           // ���B����e��q
    public Image healthBarFill;         // ��q�� UI �ϥ� Image �� fillAmount
    public GameObject deathEffect;      // ���`�S��
    public Animator animator;           // �ʵe���
    public float deathDelay = 5f;       // ���`�᩵��R�����󪺮ɶ�

    private bool isDead = false;        // �O�_�w�g���`

    void Start()
    {
        currentHealth = maxHealth;      // ��l�Ʀ�q
        UpdateHealthUI();               // ��s��q UI

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // �����k
    public void TakeDamage(int damage)
    {
        if (isDead) return;             // �p�G�w���`�A���L�޿�

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // �����q�d��
        UpdateHealthUI();               // ��s��q UI

        Debug.Log("Odin took damage: " + damage + ". Current health: " + currentHealth);

        // ������˰ʵe
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        // �p�G��q�k�s�A���榺�`�޿�
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ��s��q UI
    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth; // �ھڷ�e��q��s fillAmount
        }
    }

    // ���`�޿�
    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Odin has died!");

        // ���񦺤`�ʵe
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // �ͦ����`�S��
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // ����R������
        StartCoroutine(DestroyAfterDelay());
    }

    // ����R�����󪺨�{
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(deathDelay);
        Destroy(gameObject);
    }

    // ���եΪ���L��J��k
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // ���ծɡA�� H ���� 50 ��q
        {
            TakeDamage(50);
        }
    }
}
