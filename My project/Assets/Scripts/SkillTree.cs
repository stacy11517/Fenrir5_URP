using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Animator animator;           // 動畫控制器
    public Transform attackPoint;       // 攻擊起始點位置
    public Transform spinAttackPoint;   // 起跳旋轉攻擊起始點
    public CameraController cameraController; // 攝影機控制器
    public TriggerEvent triggerEvent;   // 連接 TriggerEvent 腳本

    // 冷卻時間
    public float dashCooldown = 1f;
    public float dualAttackCooldown = 2f;
    public float spinAttackCooldown = 3f;

    // 冷卻圖片
    public Image dashCooldownImage;
    public Image dualAttackCooldownImage;
    public Image spinAttackCooldownImage;

    // 攻擊範圍和傷害
    public float attackRange = 2f;
    public int normalAttackDamage = 5;
    public int spinAttackDamage = 15;

    // 特效
    public ParticleSystem normalAttackEffect;
    public ParticleSystem spinAttackEffect;

    private bool isPerformingSkill = false; // 是否正在執行技能

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
            HandleSpinAttack();
        }
    }

    // **普通攻擊**
    void HandleNormalAttack()
    {
        if ((Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.V)) && !isPerformingSkill)
        {
            animator.SetTrigger("NormalAttack");
            isPerformingSkill = true;

            // 播放普通攻擊特效
            if (normalAttackEffect != null)
            {
                Instantiate(normalAttackEffect, attackPoint.position, Quaternion.LookRotation(transform.forward));
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
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(normalAttackDamage);
                Debug.Log("Hit " + enemy.name);
            }
        }

        isPerformingSkill = false;
    }

    // **起跳旋轉攻擊**
    void HandleSpinAttack()
    {
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton1)) && spinAttackCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("SpinAttack");
            isPerformingSkill = true;

            // 播放起跳旋轉攻擊特效
            if (spinAttackEffect != null)
            {
                Instantiate(spinAttackEffect, spinAttackPoint.position, Quaternion.LookRotation(transform.forward));
            }

            StartCoroutine(CooldownRoutine(spinAttackCooldown, spinAttackCooldownImage));
        }
    }

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

                // 觸發攝影機震動
                cameraController.TriggerShake(0.4f, 0.2f);
            }
        }

        // 通知 TriggerEvent 記錄技能使用
        if (triggerEvent != null)
        {
            triggerEvent.RegisterSkillUse();
        }

        isPerformingSkill = false;
    }

    // **冷卻顯示協程**
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
