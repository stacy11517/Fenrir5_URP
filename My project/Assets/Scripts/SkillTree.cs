using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Animator animator;                  // Animator 元件引用
    public Transform attackPoint;              // 攻擊範圍起點
    public Transform spinAttackPoint;          // 起跳旋轉攻擊的起點

    // 技能冷卻時間
    public float dashCooldown = 1f;            // 衝刺冷卻時間
    public float dualAttackCooldown = 2f;      // 來回攻擊冷卻時間
    public float spinAttackCooldown = 3f;      // 起跳旋轉攻擊冷卻時間

    // 冷卻 UI 圖片
    public Image dashCooldownImage;
    public Image dualAttackCooldownImage;
    public Image spinAttackCooldownImage;

    // 攻擊範圍和傷害
    public float attackRange = 2f;             // 來回攻擊範圍
    public int attackDamage = 10;              // 來回攻擊傷害值
    public int spinAttackDamage = 15;          // 起跳旋轉攻擊傷害值
    public int normalAttackDamage = 5;         // 普通攻擊傷害值

    private bool isPerformingSkill = false;    // 當執行技能時防止其他技能觸發

    void Start()
    {
        animator = GetComponent<Animator>();
        ResetCooldownImages();
    }

    void Update()
    {
        if (!isPerformingSkill) // 確保無其他技能執行中
        {
            HandleDash();
            HandleDualAttack();
            HandleSpinAttack();
            HandleNormalAttack();
        }
    }

    // 普通攻擊
    void HandleNormalAttack()
    {
        if ((Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.V)) && !isPerformingSkill)
        {
            animator.SetTrigger("NormalAttack");
            isPerformingSkill = true;
            StartCoroutine(NormalAttackRoutine());
        }
    }

    IEnumerator NormalAttackRoutine()
    {
        yield return new WaitForSeconds(0.5f);  // 假設攻擊動畫播放時間為0.5秒

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(normalAttackDamage);  // 使敵人扣血
                Debug.Log("Hit " + enemy.name);
            }
        }

        isPerformingSkill = false;
    }

    // 衝刺技能
    void HandleDash()
    {
        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton0)) && dashCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("Dash");
            isPerformingSkill = true;
            StartCoroutine(PerformDash());
            StartCoroutine(CooldownRoutine(dashCooldown, dashCooldownImage));
        }
    }

    IEnumerator PerformDash()
    {
        float dashTime = 0.5f;
        float dashSpeed = 20f;
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            transform.Translate(Vector3.forward * dashSpeed * Time.deltaTime);
            yield return null;
        }
        isPerformingSkill = false;
    }

    // 來回攻擊技能
    void HandleDualAttack()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)) && dualAttackCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("DualAttack");
            isPerformingSkill = true;
            StartCoroutine(CooldownRoutine(dualAttackCooldown, dualAttackCooldownImage));
        }
    }

    public void PerformDualAttack() // Animation Event 呼叫
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                Debug.Log("Hit " + enemy.name);
            }
        }
        isPerformingSkill = false;
    }

    // 起跳旋轉攻擊技能
    void HandleSpinAttack()
    {
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton1)) && spinAttackCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("SpinAttack");
            isPerformingSkill = true;
            StartCoroutine(CooldownRoutine(spinAttackCooldown, spinAttackCooldownImage));
        }
    }

    public void PerformSpinAttack() // Animation Event 呼叫
    {
        Collider[] hitEnemies = Physics.OverlapSphere(spinAttackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(spinAttackDamage);
                Debug.Log("Hit " + enemy.name);
            }
        }
        isPerformingSkill = false;
    }

    // 冷卻顯示協程
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

    void ResetCooldownImages()
    {
        dashCooldownImage.fillAmount = 1f;
        dualAttackCooldownImage.fillAmount = 1f;
        spinAttackCooldownImage.fillAmount = 1f;
    }
}
