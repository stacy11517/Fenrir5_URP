using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OdinHealth : MonoBehaviour
{
    public int maxHealth = 500;          // 奧丁的最大血量
    public int currentHealth;           // 奧丁的當前血量
    public Image healthBarFill;         // 血量條 UI 使用 Image 的 fillAmount
    public ParticleSystem hurtEffect;   // 受傷特效
    public Animator animator;           // 動畫控制器
    public float deathDelay = 5f;       // 死亡後延遲刪除物件的時間
    public GameObject victoryScreen;    // 過關圖片（UI 面板）

    private bool isDead = false;        // 是否已經死亡
    public OdinBoss odinBossScript;     // Odin 的技能腳本引用

    public Animator fadingPanelAni;
    public LevelLoader levelLoader;

    void Start()
    {
        currentHealth = maxHealth;      // 初始化血量
        UpdateHealthUI();               // 更新血量 UI

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (odinBossScript == null)
        {
            odinBossScript = GetComponent<OdinBoss>();
        }

        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false); // 開始時隱藏過關圖片
        }
    }

    // 扣血方法
    public void TakeDamage(int damage)
    {
        if (isDead) return;             // 如果已死亡，跳過邏輯

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 限制血量範圍
        UpdateHealthUI();               // 更新血量 UI

        Debug.Log("Odin took damage: " + damage + ". Current health: " + currentHealth);

        // 播放受傷動畫
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        // 播放受傷特效
        PlayHurtEffect();

        // 如果血量歸零，執行死亡邏輯
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 更新血量 UI
    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth; // 根據當前血量更新 fillAmount
        }
    }

    // 播放受傷特效
    private void PlayHurtEffect()
    {
        if (hurtEffect != null)
        {
            ParticleSystem effectInstance = Instantiate(hurtEffect, transform.position, Quaternion.identity);
            effectInstance.Play();
            Destroy(effectInstance.gameObject, effectInstance.main.duration); // 確保特效播放後銷毀
        }
    }

    // 死亡邏輯
    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Odin has died!");

        // 停止所有技能
        if (odinBossScript != null)
        {
            odinBossScript.StopAllCoroutines(); // 停止技能協程
            odinBossScript.enabled = false;    // 禁用 OdinBoss 腳本
        }

        // 播放死亡動畫
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // 延遲刪除物件並顯示過關圖片
        StartCoroutine(HandleVictoryScreen());
    }

    // 顯示過關圖片的協程
    private IEnumerator HandleVictoryScreen()
    {
        //yield return new WaitForSeconds(deathDelay); // 等待死亡延遲時間

        //// 刪除奧丁物件
        ////Destroy(gameObject);
        ////隱藏奧丁物件
        ////this.gameObject.SetActive(false);


        // 顯示過關圖片
        yield return new WaitForSeconds(deathDelay); // 等待死亡延遲時間
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
            fadingPanelAni.SetTrigger("StartFading");
            Debug.Log("Victory Screen is now displayed!");
            Invoke("ToTextboxScene", 2.5f);
        }
    }

    // 測試用的鍵盤輸入方法
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // 測試時，按 H 鍵減少 50 血量
        {
            TakeDamage(50);
        }
    }

    void ToTextboxScene()
    {
        levelLoader.LoadNextLevel();
    }
}
