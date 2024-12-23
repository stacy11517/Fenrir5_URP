﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;          // 玩家最大生命值
    public int currentHealth;            // 玩家當前生命值
    public int healthPackCount = 0;      // 補血包的數量
    public int AddHealthAmount =75;          //補血量
    public Image healthBar;              // 血條 UI
    public TMP_Text healthPackText;      // 血量包數量的 TextMeshPro 元件
    public GameObject deathScreen;       // 死亡畫面 UI
    public Animator animator;            // 用於播放死亡動畫的 Animator
    public ParticleSystem healEffect;    // 補血時的特效

    public ParticleSystem hurtEffect;            // 受傷特效
    public float hurtEffectCooldown = 1f;        // 受傷特效的冷卻時間
    private bool canPlayHurtEffect = true;       // 控制是否可以播放受傷特效

    public Animator DeadScreenFadeIn;

    public Button DeadScreenFirstButton;
    private EventSystem eventSystem;

    public AudioClip pickupSound;
    public AudioClip healingSound;
    private AudioSource audioSource;  // 音效播放器

    private PlayerController playerController;
    public bool IsDead { get; private set; } = false;  // 玩家死亡狀態，只讀屬性

    void Start()
    {
        IsDead = false;
        currentHealth = maxHealth;      // 初始化玩家生命值
        UpdateHealthBar();              // 初始化血條
        UpdateHealthPackText();         // 初始化補血包數量文本

        if (deathScreen != null)
        {
            deathScreen.SetActive(false);   // 隱藏死亡畫面 UI

        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();  // 獲取 Animator 元件
        }

        playerController = GetComponent<PlayerController>(); // 獲取 PlayerController 元件
                                                             // 初始化 EventSystem
        eventSystem = EventSystem.current;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (!IsDead)
        {
            // 按下鍵盤 F 鍵或手把 Y 鍵（JoystickButton5）來使用補血包
            if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton5)) && healthPackCount > 0)
            {
                UseHealthPack();
            }
            // 測試按鍵 L，扣除 50 血
            if (Input.GetKeyDown(KeyCode.L))
            {
                TakeDamage(50);
            }
        }
    }

    // 恢復生命值的方法
    public void Heal(int amount)
    {
        currentHealth += amount; // 恢復生命值
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();       // 更新血條
    }

    // 當碰到補血包時觸發
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPack"))
        {
            if (pickupSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(pickupSound);
            }
            healthPackCount++;               // 補血包數量增加
            UpdateHealthPackText();          // 更新補血包文本
            Destroy(other.gameObject);       // 銷毀補血包物件
        }
    }

    // 使用補血包恢復生命
    void UseHealthPack()
    {
        if (currentHealth < maxHealth)
        {
            if (playerController != null)
            {
                playerController.canMove = false; // 禁止玩家移動
            }

            Heal(AddHealthAmount);               // 恢復生命值
            healthPackCount--;                   // 補血包數量減少
            UpdateHealthPackText();              // 更新補血包文本

            // 播放補血特效
            if (healEffect != null)
            {
                Instantiate(healEffect, transform.position, transform.rotation).Play();
            }
            if (healingSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(healingSound);
            }

            Invoke("EnableMovement", 1f);        // 延遲恢復移動
        }
    }

    void EnableMovement()
    {
        if (playerController != null)
        {
            playerController.canMove = true;
        }
    }

    // 更新血條顯示
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth; // 調整血條
        }
    }

    // 更新補血包數量文本顯示
    void UpdateHealthPackText()
    {
        if (healthPackText != null)
        {
            healthPackText.text = healthPackCount.ToString();  // 顯示補血包數量
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // 已经死亡，无法再受伤

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took damage: {damage}. Current health: {currentHealth}");

        if (hurtEffect != null && canPlayHurtEffect)
        {
            Instantiate(hurtEffect, transform.position, transform.rotation).Play();
            StartCoroutine(HurtEffectCooldownRoutine());
        }

        // 更新血量条（如果有）
        UpdateHealthBar();

        // 检查是否死亡
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    IEnumerator HurtEffectCooldownRoutine()
    {
        canPlayHurtEffect = false;
        yield return new WaitForSeconds(hurtEffectCooldown);
        canPlayHurtEffect = true;
    }

    // 設置第一個選中的按鈕
    private void SetFirstSelectedButton(Button button)
    {
        if (button != null)
        {
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(button.gameObject);
        }
    }

    // 玩家死亡邏輯
    void Die()
    {
        if (IsDead) return;

        IsDead = true; // 標記玩家為已死亡
        if (playerController != null)
        {
            playerController.canMove = false; // 禁止玩家移動
        }

        if (animator != null)
        {
            animator.SetTrigger("Die"); // 播放死亡動畫
        }

        // 延遲顯示死亡畫面
        StartCoroutine(ShowDeathScreenAfterAnimation());
    }

    IEnumerator ShowDeathScreenAfterAnimation()
    {
        // 等待死亡動畫完成
        if (animator != null)
        {
            
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("Die") &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        }


        //顯示死亡畫面
        if (deathScreen != null)
        {

            deathScreen.SetActive(true);
            // 設置死亡面板按鈕
            if (DeadScreenFirstButton != null) SetFirstSelectedButton(DeadScreenFirstButton);
        }
        Time.timeScale = 0f; // 暫停遊戲
        DeadScreenFadeIn.SetTrigger("Show");




    }
}
