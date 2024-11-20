using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint; // 攻擊起始點位置
    public Transform spinAttackPoint;
    public Transform headAttackPoint;

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
    public int attackDamage = 10;
    public int spinAttackDamage = 15;
    public int normalAttackDamage = 5;

    // 特效
    public ParticleSystem dashEffect;
    public ParticleSystem dualAttackEffect;
    public ParticleSystem spinAttackEffect;
    public ParticleSystem normalAttackEffect;

    private bool isPerformingSkill = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        ResetCooldownImages();
    }

    void Update()
    {
        if (!isPerformingSkill)
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

            // 播放普通攻擊特效
            if (normalAttackEffect != null)
            {
                var effect = Instantiate(normalAttackEffect, attackPoint.position, Quaternion.LookRotation(transform.forward));
                effect.transform.Rotate(Vector3.up, 0f); // 根据需要进一步调整特效自身的旋转角度
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

    // 衝刺技能
    void HandleDash()
    {
        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton0)) && dashCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("Dash");
            isPerformingSkill = true;

            // 播放衝刺特效
            if (dashEffect != null)
            {
                var effect = Instantiate(dashEffect, transform.position, Quaternion.LookRotation(transform.forward));
                effect.transform.Rotate(Vector3.up, 0f); // 根据需要进一步调整特效自身的旋转角度
            }

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

    // 來回攻擊技能 (Dual Attack)
    void HandleDualAttack()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)) && dualAttackCooldownImage.fillAmount == 1f)
        {
            animator.SetTrigger("DualAttack");
            isPerformingSkill = true;

            // 播放來回攻擊特效
            if (dualAttackEffect != null)
            {
                var effect = Instantiate(dualAttackEffect, attackPoint.position, Quaternion.LookRotation(transform.forward));
                effect.transform.Rotate(Vector3.up, 0f); // 根据需要进一步调整特效自身的旋转角度
            }

            StartCoroutine(CooldownRoutine(dualAttackCooldown, dualAttackCooldownImage));
        }
    }

    // 這個方法會在 Dual Attack 的動畫播放過程中被調用
    public void PerformDualAttack()
    {
        // 使用 Raycast 進行直線攻擊判定
        RaycastHit hit;
        Vector3 direction = transform.forward; // 攻擊方向為角色面朝的方向

        if (Physics.Raycast(attackPoint.position, direction, out hit, attackRange))
        {
            // 如果射線擊中目標，檢查是否是敵人
            EnemyController enemyController = hit.collider.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(attackDamage);
                Debug.Log("Hit " + hit.collider.name);
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

            // 播放起跳旋轉攻擊特效
            if (spinAttackEffect != null)
            {
                var effect = Instantiate(spinAttackEffect, headAttackPoint.position, Quaternion.LookRotation(transform.forward));
                effect.transform.Rotate(Vector3.up, 0f); // 根据需要进一步调整特效自身的旋转角度
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
