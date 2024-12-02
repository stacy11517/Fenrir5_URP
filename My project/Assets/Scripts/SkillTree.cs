using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Animator animator;
    public Transform spinAttackPoint;
    public float spinAttackCooldown = 3f;
    public Image spinAttackCooldownImage;
    public float attackRange = 2f;
    public int spinAttackDamage = 15;
    public ParticleSystem spinAttackEffect;

    private bool isPerformingSkill = false;

    // 新增一個對 TriggerEvent 的引用
    public TriggerEvent triggerEvent;

    void Start()
    {
        animator = GetComponent<Animator>();
        ResetCooldownImages();
    }

    void Update()
    {
        if (!isPerformingSkill)
        {
            HandleSpinAttack();
        }
    }

    // 起跳旋轉攻擊技能
    void HandleSpinAttack()
    {
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton1)) && spinAttackCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("SpinAttack");
            isPerformingSkill = true;

            // 播放起跳旋轉攻擊特效
            if (spinAttackEffect != null)
            {
                var effect = Instantiate(spinAttackEffect, spinAttackPoint.position, Quaternion.LookRotation(transform.forward));
                effect.transform.Rotate(Vector3.up, 0f); // 根據需要進一步調整特效自身的旋轉角度
            }

            // 記錄冷卻時間
            StartCoroutine(CooldownRoutine(spinAttackCooldown, spinAttackCooldownImage));
        }
    }

    // 由動畫事件觸發的旋轉攻擊方法
    public void PerformSpinAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(spinAttackPoint.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(spinAttackDamage);
                Debug.Log("Hit " + enemy.name);
            }
        }

        // 在技能觸發時購用 TriggerEvent 中的 RegisterSkillUse
        if (triggerEvent != null)
        {
            triggerEvent.RegisterSkillUse();
        }

        isPerformingSkill = false; // 技能結束後重置狀態
    }

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
        spinAttackCooldownImage.fillAmount = 1f;
    }
}
