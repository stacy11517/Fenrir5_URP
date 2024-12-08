using System.Collections;
using UnityEngine;

public class OdinBoss : MonoBehaviour
{
    public Transform player; // 玩家位置
    public GameObject thunderBallPrefab; // 雷球特效預製件
    public Transform throwPoint; // 雷球丟出的起點
    public LightningStrike lightningStrikeManager; // 雷擊範圍管理器
    public float thunderBallCooldown = 7f; // 雷球技能的冷卻時間
    public float thunderBallSpeed = 15f; // 雷球的飛行速度
    public int thunderBallDamage = 50; // 雷球的傷害

    private bool canUseLightning = true;
    private bool canThrowThunderBall = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // 檢查 LightningStrike 是否已經連接
        if (lightningStrikeManager == null)
        {
            Debug.LogError("LightningStrike 管理器未設置！");
        }
    }

    void Update()
    {
        // 觸發地圖雷擊技能
        if (canUseLightning && lightningStrikeManager != null)
        {
            StartCoroutine(UseLightningStrike());
        }

        // 觸發雷球技能
        if (canThrowThunderBall)
        {
            StartCoroutine(ThrowThunderBall());
        }
    }

    // 地圖雷擊技能
    IEnumerator UseLightningStrike()
    {
        canUseLightning = false;
        animator.SetTrigger("UseLightning"); // 播放雷擊動畫

        yield return new WaitForSeconds(1f); // 等待動畫前置部分

        // 調用 LightningStrike 管理器來觸發雷擊
        lightningStrikeManager.TriggerLightning();

        yield return new WaitForSeconds(lightningStrikeManager.lightningCooldown);
        canUseLightning = true;
    }

    // 雷球攻擊技能
    IEnumerator ThrowThunderBall()
    {
        canThrowThunderBall = false;
        animator.SetTrigger("ThrowThunderBall"); // 播放雷球攻擊動畫

        yield return new WaitForSeconds(0.5f); // 等待動畫前置部分

        // 創建雷球並設置其方向朝向玩家
        GameObject thunderBall = Instantiate(thunderBallPrefab, throwPoint.position, Quaternion.identity);
        Vector3 direction = (player.position - throwPoint.position).normalized;
        Rigidbody rb = thunderBall.GetComponent<Rigidbody>();
        rb.velocity = direction * thunderBallSpeed;

        // 設置雷球飛行時間
        Destroy(thunderBall, 5f);

        yield return new WaitForSeconds(thunderBallCooldown);
        canThrowThunderBall = true;
    }

    // 當雷球碰撞到玩家時觸發傷害
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.name.Contains("ThunderBall"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(thunderBallDamage);
                Debug.Log("Player hit by Thunder Ball! Took " + thunderBallDamage + " damage.");
            }

            // 銷毀雷球
            Destroy(other.gameObject);
        }
    }
}
