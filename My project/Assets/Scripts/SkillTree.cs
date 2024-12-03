using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;         // 普通攻擊起始點
    public Transform dualAttackPoint;    // 來回攻擊起始點
    public Transform spinAttackPoint;    // 旋轉攻擊起始點

    public float dashCooldown = 1f;      // 衝刺冷卻時間
    public float dualAttackCooldown = 2f; // 來回攻擊冷卻時間
    public float spinAttackCooldown = 3f; // 起跳旋轉冷卻時間

    public Image dashCooldownImage;
    public Image dualAttackCooldownImage;
    public Image spinAttackCooldownImage;

    public float attackRange = 2f;       // 攻擊範圍
    public int normalAttackDamage = 5;  // 普通攻擊傷害
    public int dualAttackDamage = 10;   // 來回攻擊傷害
    public int spinAttackDamage = 15;   // 起跳旋轉攻擊傷害

    public ParticleSystem dashEffect;
    public ParticleSystem dualAttackEffect;
    public ParticleSystem spinAttackEffect;
    public ParticleSystem normalAttackEffect;

    private bool isPerformingSkill = false;

    public TriggerEvent triggerEvent; // 觸發特定事件的引用

    void Start()
    {
        animator = GetComponent<Animator>();
        ResetCooldownImages();
    }

    void Update()
    {
        if (!isPerformingSkill)
        {
            HandleNormalAttack();
            HandleDash();
            HandleDualAttack();
            HandleSpinAttack();
        }
    }

    // 普通攻擊
    void HandleNormalAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // 左鍵或其他鍵觸發普通攻擊
        {
            animator.SetTrigger("NormalAttack");
            isPerformingSkill = true;

            if (normalAttackEffect != null)
            {
                var effect = Instantiate(normalAttackEffect, attackPoint.position, Quaternion.identity);
                Destroy(effect.gameObject, 1f);
            }

            StartCoroutine(NormalAttackRoutine());
        }
    }

    IEnumerator NormalAttackRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out EnemyController enemyController))
            {
                enemyController.TakeDamage(normalAttackDamage);
            }
        }

        isPerformingSkill = false;
    }

    // 衝刺技能
    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("Dash");
            isPerformingSkill = true;

            if (dashEffect != null)
            {
                Instantiate(dashEffect, transform.position, Quaternion.identity);
            }

            StartCoroutine(PerformDash());
            StartCoroutine(CooldownRoutine(dashCooldown, dashCooldownImage));
        }
    }

    IEnumerator PerformDash()
    {
        float dashTime = 0.5f;
        float dashSpeed = 10f;

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
        if (Input.GetKeyDown(KeyCode.E) && dualAttackCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("DualAttack");
            isPerformingSkill = true;

            if (dualAttackEffect != null)
            {
                Instantiate(dualAttackEffect, dualAttackPoint.position, Quaternion.identity);
            }

            StartCoroutine(PerformDualAttack());
            StartCoroutine(CooldownRoutine(dualAttackCooldown, dualAttackCooldownImage));
        }
    }

    IEnumerator PerformDualAttack()
    {
        yield return new WaitForSeconds(0.5f);

        Collider[] hitEnemies = Physics.OverlapSphere(dualAttackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out EnemyController enemyController))
            {
                enemyController.TakeDamage(dualAttackDamage);
            }
        }

        isPerformingSkill = false;
    }

    // 起跳旋轉攻擊技能
    void HandleSpinAttack()
    {
        if (Input.GetKeyDown(KeyCode.Q) && spinAttackCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("SpinAttack");
            isPerformingSkill = true;

            if (spinAttackEffect != null)
            {
                Instantiate(spinAttackEffect, spinAttackPoint.position, Quaternion.identity);
            }

            StartCoroutine(CooldownRoutine(spinAttackCooldown, spinAttackCooldownImage));
        }
    }

    // 動畫事件觸發的旋轉攻擊
    public void PerformSpinAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(spinAttackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out EnemyController enemyController))
            {
                enemyController.TakeDamage(spinAttackDamage);
            }
        }

        if (triggerEvent != null)
        {
            triggerEvent.RegisterSkillUse();
        }

        isPerformingSkill = false;
    }

    // 冷卻顯示
    IEnumerator CooldownRoutine(float cooldownTime, Image cooldownImage)
    {
        cooldownImage.fillAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            cooldownImage.fillAmount = Mathf.Clamp01(elapsedTime / cooldownTime);
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
