using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    // 动画
    public Animator animator;

    // 普通攻击属性
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackAngle = 120f;
    public int normalAttackDamage = 10;
    public ParticleSystem normalAttackEffect;

    // 冲刺技能属性
    public float dashSpeed = 15f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 2f;
    public Image dashCooldownImage;
    public ParticleSystem dashEffect;

    // 来回冲刺技能属性
    public float doubleDashSpeed = 15f;
    public float doubleDashDuration = 0.3f;
    public float doubleDashCooldown = 5f;
    public int doubleDashDamage = 20;
    public float doubleDashRange = 0.2f;
    public Image doubleDashCooldownImage;
    public ParticleSystem doubleDashEffect;

    // 起跳旋转攻击技能属性
    public float spinAttackCooldown = 7f;
    public int spinAttackDamage = 20;
    public float spinAttackRange = 4f;
    public Image spinAttackCooldownImage;
    public ParticleSystem spinAttackEffect;

    private CharacterController characterController;
    private bool isPerformingSkill = false;

    public TriggerEvent triggerEvent; // 引用 TriggerEvent

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController is missing on the player!");
        }

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

    // 普通攻击
    void HandleNormalAttack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            animator.SetTrigger("NormalAttack");
        }
    }

    // Animator Event: 普通攻擊造成傷害
    public void PerformNormalAttack()
    {
        Collider[] hitTargets = Physics.OverlapSphere(attackPoint.position, attackRange);
        foreach (Collider hit in hitTargets)
        {
            // 判定是否為敵人或奧丁
            if (hit.CompareTag("Enemy") || hit.CompareTag("Odin"))
            {
                Vector3 directionToTarget = (hit.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                if (angleToTarget <= attackAngle / 2)
                {
                    // 如果是普通敵人
                    EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(normalAttackDamage);
                    }

                    // 如果是奧丁
                    OdinHealth odinHealth = hit.GetComponent<OdinHealth>();
                    if (odinHealth != null)
                    {
                        odinHealth.TakeDamage(normalAttackDamage);
                        Debug.Log("Odin took " + normalAttackDamage + " damage from Normal Attack.");
                    }
                }
            }
        }

        PlayEffect(normalAttackEffect, attackPoint.position);
    }


    // 冲刺技能
    void HandleDash()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton0)) && dashCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("Dash");
            isPerformingSkill = true;

            PlayEffect(dashEffect, transform.position);
            StartCoroutine(PerformDash());
            StartCoroutine(CooldownRoutine(dashCooldown, dashCooldownImage));
        }
    }

    IEnumerator PerformDash()
    {
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

    // 来回冲刺技能
    void HandleDoubleDash()
    {
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton3)) && doubleDashCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("DoubleDash"); // 播放动画
            isPerformingSkill = true;

            // 播放特效（初始位置）
            PlayEffect(doubleDashEffect, transform.position);

            // 开始冷却计时
            StartCoroutine(CooldownRoutine(doubleDashCooldown, doubleDashCooldownImage));
        }
    }

    // 動畫事件：觸發第一次衝刺
    public void PerformFirstDash()
    {
        StartCoroutine(DashMovement(doubleDashSpeed, doubleDashDuration));
    }

    // 動畫事件：觸發第二次衝刺
    public void PerformSecondDash()
    {
        StartCoroutine(DashMovement(-doubleDashSpeed, doubleDashDuration));
    }

    // 衝刺的位移和攻擊邏輯
    IEnumerator DashMovement(float speed, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 dashDirection = transform.forward * speed;
            characterController.Move(dashDirection * Time.deltaTime);

            // 攻擊判定
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, doubleDashRange);
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.CompareTag("Enemy") || enemy.CompareTag("Odin"))
                {
                    // 如果是普通敵人
                    EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(doubleDashDamage);
                    }

                    // 如果是奧丁
                    OdinHealth odinHealth = enemy.GetComponent<OdinHealth>();
                    if (odinHealth != null)
                    {
                        odinHealth.TakeDamage(doubleDashDamage);
                        Debug.Log("Odin took " + doubleDashDamage + " damage from Double Dash.");
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 如果是第二次衝刺，技能完成
        if (speed < 0)
        {
            isPerformingSkill = false; // 技能結束
        }
    }


    // 起跳旋转攻击
    void HandleSpinAttack()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1)) && spinAttackCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("SpinAttack"); // 触发动画
            isPerformingSkill = true;


            // 开始冷却计时
            StartCoroutine(CooldownRoutine(spinAttackCooldown, spinAttackCooldownImage));

            // 通知 TriggerEvent（如果有事件系统）
            if (triggerEvent != null)
            {
                triggerEvent.RegisterSkillUse();
            }
        }
    }

    // 动画事件：触发旋转攻击的实际效果
    public void PerformSpinAttack()
    {
        // 播放起始特效
        PlayEffect(spinAttackEffect, transform.position);
        // 检查范围内的敌人
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, spinAttackRange);
        foreach (Collider hit in hitEnemies)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Odin"))
            {
                // 如果是普通敵人
                EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(spinAttackDamage);
                }

                // 如果是奧丁
                OdinHealth odinHealth = hit.GetComponent<OdinHealth>();
                if (odinHealth != null)
                {
                    odinHealth.TakeDamage(spinAttackDamage);
                    Debug.Log("Odin took " + spinAttackDamage + " damage from Spin Attack.");
                }
            }
        }

        
    }

    // 动画事件：技能完成时调用
    public void EndSpinAttack()
    {
        isPerformingSkill = false; // 技能完成，恢复正常状态
    }


    // 公共方法：播放特效
    void PlayEffect(ParticleSystem effect, Vector3 position)
    {
        if (effect != null)
        {
            Instantiate(effect, position, transform.rotation).Play();
        }
    }

    // 公共方法：冷却计时
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

    // 初始化冷却图片
    void ResetCooldownImages()
    {
        if (dashCooldownImage != null) dashCooldownImage.fillAmount = 1f;
        if (doubleDashCooldownImage != null) doubleDashCooldownImage.fillAmount = 1f;
        if (spinAttackCooldownImage != null) spinAttackCooldownImage.fillAmount = 1f;
    }

    // 调试用：绘制攻击范围
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
