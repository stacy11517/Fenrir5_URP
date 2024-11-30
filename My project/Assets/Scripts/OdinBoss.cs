using System.Collections;
using UnityEngine;

public class OdinBoss : MonoBehaviour
{
    public Transform player; // ���a��m
    public GameObject lightningStrikePrefab; // �a�Ϲp���S�Ĺw�s��
    public GameObject thunderBallPrefab; // �p�y�S�Ĺw�s��
    public Transform throwPoint; // �p�y��X���_�I
    public float lightningCooldown = 5f; // �a�Ϲp���ޯ઺�N�o�ɶ�
    public float thunderBallCooldown = 7f; // �p�y�ޯ઺�N�o�ɶ�
    public float lightningRange = 10f; // �p���������d��
    public float thunderBallSpeed = 15f; // �p�y������t��
    public int lightningDamage = 20; // �p�����ˮ`
    public int thunderBallDamage = 50; // �p�y���ˮ`

    private bool canUseLightning = true;
    private bool canThrowThunderBall = true;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canUseLightning)
        {
            StartCoroutine(UseLightningStrike());
        }

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

        // �T�w�p���������H����m�A�d�򬰶��B�P��
        Vector3 randomPosition = new Vector3(
            transform.position.x + Random.Range(-lightningRange, lightningRange),
            transform.position.y,
            transform.position.z + Random.Range(-lightningRange, lightningRange)
        );

        // �ͦ��p���S��
        Instantiate(lightningStrikePrefab, randomPosition, Quaternion.identity);

        // �ˬd���a�O�_�b�p���d�򤺡A�æ���
        float distanceToPlayer = Vector3.Distance(randomPosition, player.position);
        if (distanceToPlayer <= 3f) // ���]�p���d��b�|��3���
        {
            player.GetComponent<PlayerHealth>().TakeDamage(lightningDamage);
            Debug.Log("Odin used Lightning Strike! Player took " + lightningDamage + " damage.");
        }

        yield return new WaitForSeconds(lightningCooldown);
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.name.Contains("ThunderBall"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(thunderBallDamage);
                Debug.Log("Player hit by Thunder Ball! Took " + thunderBallDamage + " damage.");
            }

            // �P���p�y
            Destroy(collision.gameObject);
        }
    }
}
