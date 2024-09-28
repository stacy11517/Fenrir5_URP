using UnityEngine;
using UnityEngine.UI;          // ノ UI 巨
using TMPro;                 // ノ TextMeshPro

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;          // 產程ネ㏑
    public int currentHealth;             // 產讽玡ネ㏑
    public int healthPackCount = 0;       // 干﹀计秖
    public Image healthBar;                // ﹀兵 UI
    public TMP_Text healthPackText;        // ﹀秖计秖 TextMeshPro じン

    void Start()
    {
        // ﹍て產ネ㏑
        currentHealth = maxHealth;
        UpdateHealthBar();              // ﹍て﹀兵
        UpdateHealthPackText();         // ﹍て干﹀计秖ゅセ
    }

    // 確ネ㏑よ猭
    public void Heal(int amount)
    {
        // 確ネ㏑ぃ禬筁程ネ㏑
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player healed! Current health: " + currentHealth);
        UpdateHealthBar();              // 穝﹀兵
    }

    // 讽窱干﹀牟祇
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealthPack"))
        {
            // 具癬干﹀计秖糤
            healthPackCount++;
            Debug.Log("Health pack picked up! Total packs: " + healthPackCount);
            UpdateHealthPackText();       // 穝干﹀计秖ゅセ

            // 綪反干﹀ン
            Destroy(other.gameObject);
        }
    }

    // ㄏノ干﹀
    void Update()
    {
        //  X 龄ㄓㄏノ干﹀
        if (Input.GetKeyDown(KeyCode.X) && healthPackCount > 0)
        {
            UseHealthPack();
        }
    }

    // ㄏノ干﹀確ネ㏑
    void UseHealthPack()
    {
        if (currentHealth < maxHealth)
        {
            Heal(10);                    // 確10翴ネ㏑
            healthPackCount--;           // 干﹀计秖搭ぶ
            Debug.Log("Health pack used! Remaining packs: " + healthPackCount);
            UpdateHealthPackText();      // 穝干﹀计秖ゅセ
        }
        else
        {
            Debug.Log("Health is full, cannot use health pack.");
        }
    }

    // 穝﹀兵陪ボ
    void UpdateHealthBar()
    {
        float healthPercent = (float)currentHealth / maxHealth;
        healthBar.fillAmount = healthPercent;  // 秸俱﹀兵
    }

    // 穝干﹀计秖ゅセ陪ボ
    void UpdateHealthPackText()
    {
        healthPackText.text = healthPackCount > 0 ? healthPackCount.ToString() : "";  // 讽计秖 0 陪ボ计玥ぃ陪ボ
    }
}
