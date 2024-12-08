using System.Collections;
using UnityEngine;

public class LightningStrike : MonoBehaviour
{
    public float lightningCooldown = 5f; // �C���p�����N�o�ɶ�
    public GameObject lightningPrefab; // �p���S�Ĺw�s��
    public GameObject warningCirclePrefab; // �a���ˮ`�d����ܹw�s��
    public Transform platformCenter; // ��Υ��x�������I
    public float platformRadius = 10f; // ��Υ��x���b�|
    public int minStrikes = 2; // �̤p�p���ƶq
    public int maxStrikes = 3; // �̤j�p���ƶq
    public float warningDuration = 1.5f; // �ˮ`�d����ܫ���ɶ�
    public int lightningDamage = 20; // �p���ˮ`
    public float strikeRadius = 5f; // �p���ˮ`�d��b�|

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // ������a
    }

    public void TriggerLightning()
    {
        StartCoroutine(GenerateLightningStrikes());
    }

    IEnumerator GenerateLightningStrikes()
    {
        // �H���M�w�ͦ����p���ƶq
        int strikeCount = Random.Range(minStrikes, maxStrikes + 1);

        for (int i = 0; i < strikeCount; i++)
        {
            // �p���H����m
            Vector3 randomPosition = GetRandomPointInCircle();

            // �b�H����m�ͦ�ĵ�i�d��
            GameObject warning = Instantiate(warningCirclePrefab, randomPosition, Quaternion.Euler(0, 0, 0));
            // �O�� Y �b���ܡA�Ƚվ� X �M Z �b���Y��
            Vector3 currentScale = warning.transform.localScale; // �����e���Y��
            warning.transform.localScale = new Vector3(strikeRadius * 2, currentScale.y, strikeRadius * 2);


            yield return new WaitForSeconds(warningDuration);

            // �bĵ�i�d���m�ͦ��p��
            Instantiate(lightningPrefab, randomPosition, Quaternion.identity);

            // �ˬd���a�O�_�b�p���d��
            float distanceToPlayer = Vector3.Distance(player.position, randomPosition);
            if (distanceToPlayer <= strikeRadius)
            {
                player.GetComponent<PlayerHealth>()?.TakeDamage(lightningDamage);
                Debug.Log("Player hit by lightning! Took " + lightningDamage + " damage.");
            }

            // �P��ĵ�i�d��
            Destroy(warning);
        }
    }

    Vector3 GetRandomPointInCircle()
    {
        // �H�����˷�����
        float angle = Random.Range(0f, Mathf.PI * 2);
        float radius = Random.Range(0f, platformRadius);
        return new Vector3(
            platformCenter.position.x + Mathf.Cos(angle) * radius,
            platformCenter.position.y,
            platformCenter.position.z + Mathf.Sin(angle) * radius
        );
    }
}
