using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int healthPackCount = 0;
    public Image healthBar;
    public TMP_Text healthPackText;
    public GameObject deathScreenUI;      // 死亡畫面 UI

    private Animator animator;            // Animator 用於控制動畫
    private bool isDead = false;          // 紀錄玩家是否已經死亡

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthBar();
        UpdateHealthPackText();
        deathScreenUI.SetActive(false);  // 初始化時隱藏死亡畫面 UI
    }

    void Update()
    {
        if (isDead && Input.anyKeyDown)
        {
            // 重新加載編號為 0 的場景
            SceneManager.LoadScene(0);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        isDead = true;

        // 觸發死亡動畫
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // 顯示死亡畫面 UI
        deathScreenUI.SetActive(true);

        // 禁用玩家控制腳本
        DisablePlayerControls();
    }

    void DisablePlayerControls()
    {
        // 在這裡禁用玩家的控制腳本
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        SkillTree skillTree = GetComponent<SkillTree>();
        if (skillTree != null)
        {
            skillTree.enabled = false;
        }
    }

    void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthBar.fillAmount = healthPercent;
    }

    void UpdateHealthPackText()
    {
        healthPackText.text = healthPackCount.ToString();
    }
}
