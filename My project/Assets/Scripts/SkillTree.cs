using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    // 動畫
    public Animator animator;

    // 攻擊屬性
    public Transform attackPoint;
    public float attackRange = 1f;            // 普通攻擊範圍
    public float attackAngle = 45f;          // 普通攻擊角度
    public int normalAttackDamage = 10;      // 普通攻擊傷害
    public ParticleSystem normalAttackEffect; // 普通攻擊特效

    // 衝刺技能屬性
    public float dashSpeed = 15f;            // 純粹衝刺速度
    public float dashDuration = 0.3f;        // 衝刺持續時間
    public float dashCooldown = 2f;          // 衝刺冷卻時間
    public Image dashCooldownImage;
    public ParticleSystem dashEffect;         // 衝刺特效

    // 來回衝刺技能屬性
    public float doubleDashSpeed = 20f;      // 來回衝刺速度
    public float doubleDashDuration = 0.5f; // 每次衝刺持續時間
    public float doubleDashCooldown = 5f;   // 來回衝刺冷卻時間
    public int doubleDashDamage = 25;       // 來回衝刺傷害
    public Image doubleDashCooldownImage;
    public ParticleSystem doubleDashEffect; // 來回衝刺特效

    // 起跳旋轉攻擊技能屬性
    public float spinAttackCooldown = 7f;    // 起跳旋轉攻擊冷卻時間
    public int spinAttackDamage = 20;        // 起跳旋轉攻擊傷害
    public float spinAttackRange = 2.5f;     // 起跳旋轉攻擊範圍
    public Image spinAttackCooldownImage;
    public ParticleSystem spinAttackEffect; // 起跳旋轉攻擊特效

    private CharacterController characterController;
    private bool isPerformingSkill = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        ResetCooldownImages();
    }

    void Update()
    {
        if (!isPerformingSkill)
        {
            HandleNormalAttack();
            HandleDash();
            HandleDoubleDash();
            HandleSpinAttack();
        }
    }

    // 普通攻擊
    void HandleNormalAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            animator.SetTrigger("NormalAttack");
            StartCoroutine(PerformNormalAttack());
        }
    }

    IEnumerator PerformNormalAttack()
    {
        yield return new WaitForSeconds(0.3f); // 動畫前置時間

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (Collider hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector3 directionToEnemy = (hit.transform.position - transform.position).normalized;
                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);
                if (angleToEnemy <= attackAngle / 2)
                {
                    EnemyController enemyController = hit.GetComponent<EnemyController>();
                    if (enemyController != null)
                    {
                        enemyController.TakeDamage(normalAttackDamage);
                    }
                }
            }
        }

        // 播放普通攻擊特效
        if (normalAttackEffect != null)
        {
            Instantiate(normalAttackEffect, attackPoint.position, transform.rotation).Play();
        }
    }

    // 純粹衝刺技能
    void HandleDash()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton0)) && dashCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("Dash");
            isPerformingSkill = true;

            StartCoroutine(PerformDash());
            StartCoroutine(CooldownRoutine(dashCooldown, dashCooldownImage));
        }
    }

    IEnumerator PerformDash()
    {
        if (dashEffect != null)
        {
            Instantiate(dashEffect, transform.position, transform.rotation).Play();
        }

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            Vector3 dashDirection = transform.forward * dashSpeed;
            characterController.Move(dashDirection * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isPerformingSkill = false;
    }

    // 來回衝刺技能
    void HandleDoubleDash()
    {
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton3)) && doubleDashCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("DoubleDash");
            isPerformingSkill = true;

            if (doubleDashEffect != null)
            {
                Instantiate(doubleDashEffect, transform.position, transform.rotation).Play();
            }

            StartCoroutine(PerformDoubleDash());
            StartCoroutine(CooldownRoutine(doubleDashCooldown, doubleDashCooldownImage));
        }
    }

    IEnumerator PerformDoubleDash()
    {
        // 第一次衝刺
        yield return PerformDashAndAttack(doubleDashSpeed, doubleDashDuration);

        // 短暫停頓
        yield return new WaitForSeconds(0.2f);

        // 第二次衝刺返回
        yield return PerformDashAndAttack(-doubleDashSpeed, doubleDashDuration);

        isPerformingSkill = false;
    }

    IEnumerator PerformDashAndAttack(float speed, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 dashDirection = transform.forward * speed;
            characterController.Move(dashDirection * Time.deltaTime);

            // 檢測攻擊範圍內的敵人
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    if (enemyController != null)
                    {
                        enemyController.TakeDamage(doubleDashDamage);
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // 起跳旋轉攻擊
    void HandleSpinAttack()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1)) && spinAttackCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("SpinAttack");
            isPerformingSkill = true;

            if (spinAttackEffect != null)
            {
                Instantiate(spinAttackEffect, transform.position, transform.rotation).Play();
            }

            StartCoroutine(PerformSpinAttack());
            StartCoroutine(CooldownRoutine(spinAttackCooldown, spinAttackCooldownImage));
        }
    }

    IEnumerator PerformSpinAttack()
    {
        yield return new WaitForSeconds(0.5f);

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, spinAttackRange);
        foreach (Collider hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyController enemyController = hit.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.TakeDamage(spinAttackDamage);
                }
            }
        }

        isPerformingSkill = false;
    }

    // 冷卻邏輯
    IEnumerator CooldownRoutine(float cooldownTime, Image cooldownImage)
    {
        cooldownImage.fillAmount = 0f;
        float elapsed = 0f;

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime;
            cooldownImage.fillAmount = Mathf.Clamp01(elapsed / cooldownTime);
            yield return null;
        }

        cooldownImage.fillAmount = 1f;
    }

    // 初始化冷卻圖片
    void ResetCooldownImages()
    {
        if (dashCooldownImage != null) dashCooldownImage.fillAmount = 1f;
        if (doubleDashCooldownImage != null) doubleDashCooldownImage.fillAmount = 1f;
        if (spinAttackCooldownImage != null) spinAttackCooldownImage.fillAmount = 1f;
    }

    // 繪製攻擊範圍（僅用於調試）
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spinAttackRange);
    }
}
