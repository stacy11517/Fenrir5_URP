using System.Collections;
using UnityEngine;

public class OdinBoss : MonoBehaviour
{
    public Transform player; // ���a��m
    public GameObject thunderBallPrefab; // �p�y�S�Ĺw�s��
    public Transform throwPoint; // �p�y��X���_�I
    public LightningStrike lightningStrikeManager; // �p���d��޲z��
    public float thunderBallCooldown = 7f; // �p�y�ޯ઺�N�o�ɶ�
    public float thunderBallSpeed = 15f; // �p�y������t��
    public int thunderBallDamage = 50; // �p�y���ˮ`

    private bool canUseLightning = true;
    private bool canThrowThunderBall = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // �ˬd LightningStrike �O�_�w�g�s��
        if (lightningStrikeManager == null)
        {
            Debug.LogError("LightningStrike �޲z�����]�m�I");
        }
    }

    void Update()
    {
        // Ĳ�o�a�Ϲp���ޯ�
        if (canUseLightning && lightningStrikeManager != null)
        {
            StartCoroutine(UseLightningStrike());
        }

        // Ĳ�o�p�y�ޯ�
        if (canThrowThunderBall)
        {
            StartCoroutine(ThrowThunderBall());
        }
    }

    // �a�Ϲp���ޯ�
    IEnumerator UseLightningStrike()
    {
        canUseLightning = false;
        animator.SetTrigger("UseLightning"); // ����p���ʵe

        yield return new WaitForSeconds(1f); // ���ݰʵe�e�m����

        // �ե� LightningStrike �޲z����Ĳ�o�p��
        lightningStrikeManager.TriggerLightning();

        yield return new WaitForSeconds(lightningStrikeManager.lightningCooldown);
        canUseLightning = true;
    }

    // �p�y�����ޯ�
    IEnumerator ThrowThunderBall()
    {
        canThrowThunderBall = false;
        animator.SetTrigger("ThrowThunderBall"); // ����p�y�����ʵe

        yield return new WaitForSeconds(0.5f); // ���ݰʵe�e�m����

        // �Ыعp�y�ó]�m���V�¦V���a
        GameObject thunderBall = Instantiate(thunderBallPrefab, throwPoint.position, Quaternion.identity);
        Vector3 direction = (player.position - throwPoint.position).normalized;
        Rigidbody rb = thunderBall.GetComponent<Rigidbody>();
        rb.velocity = direction * thunderBallSpeed;

        // �]�m�p�y����ɶ�
        Destroy(thunderBall, 5f);

        yield return new WaitForSeconds(thunderBallCooldown);
        canThrowThunderBall = true;
    }

    // ��p�y�I���쪱�a��Ĳ�o�ˮ`
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

            // �P���p�y
            Destroy(other.gameObject);
        }
    }
}
