using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OdinHealth : MonoBehaviour
{
    public int maxHealth = 500;          // ���B���̤j��q
    public int currentHealth;           // ���B����e��q
    public Image healthBarFill;         // ��q�� UI �ϥ� Image �� fillAmount
    public ParticleSystem hurtEffect;   // ���˯S��
    public Animator animator;           // �ʵe���
    public float deathDelay = 5f;       // ���`�᩵��R�����󪺮ɶ�
    public GameObject victoryScreen;    // �L���Ϥ��]UI ���O�^

    private bool isDead = false;        // �O�_�w�g���`
    public OdinBoss odinBossScript;     // Odin ���ޯ�}���ޥ�

    public Animator fadingPanelAni;
    public LevelLoader levelLoader;

    void Start()
    {
        currentHealth = maxHealth;      // ��l�Ʀ�q
        UpdateHealthUI();               // ��s��q UI

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
            victoryScreen.SetActive(false); // �}�l�����ùL���Ϥ�
        }
    }

    // �����k
    public void TakeDamage(int damage)
    {
        if (isDead) return;             // �p�G�w���`�A���L�޿�

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // �����q�d��
        UpdateHealthUI();               // ��s��q UI

        Debug.Log("Odin took damage: " + damage + ". Current health: " + currentHealth);

        // ������˰ʵe
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }

        // ������˯S��
        PlayHurtEffect();

        // �p�G��q�k�s�A���榺�`�޿�
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ��s��q UI
    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth; // �ھڷ�e��q��s fillAmount
        }
    }

    // ������˯S��
    private void PlayHurtEffect()
    {
        if (hurtEffect != null)
        {
            ParticleSystem effectInstance = Instantiate(hurtEffect, transform.position, Quaternion.identity);
            effectInstance.Play();
            Destroy(effectInstance.gameObject, effectInstance.main.duration); // �T�O�S�ļ����P��
        }
    }

    // ���`�޿�
    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Odin has died!");

        // ����Ҧ��ޯ�
        if (odinBossScript != null)
        {
            odinBossScript.StopAllCoroutines(); // ����ޯ��{
            odinBossScript.enabled = false;    // �T�� OdinBoss �}��
        }

        // ���񦺤`�ʵe
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // ����R���������ܹL���Ϥ�
        StartCoroutine(HandleVictoryScreen());
    }

    // ��ܹL���Ϥ�����{
    private IEnumerator HandleVictoryScreen()
    {
        //yield return new WaitForSeconds(deathDelay); // ���ݦ��`����ɶ�

        //// �R�����B����
        ////Destroy(gameObject);
        ////���ö��B����
        ////this.gameObject.SetActive(false);


        // ��ܹL���Ϥ�
        yield return new WaitForSeconds(deathDelay); // ���ݦ��`����ɶ�
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
            fadingPanelAni.SetTrigger("StartFading");
            Debug.Log("Victory Screen is now displayed!");
            Invoke("ToTextboxScene", 2.5f);
        }
    }

    // ���եΪ���L��J��k
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // ���ծɡA�� H ���� 50 ��q
        {
            TakeDamage(50);
        }
    }

    void ToTextboxScene()
    {
        levelLoader.LoadNextLevel();
    }
}
