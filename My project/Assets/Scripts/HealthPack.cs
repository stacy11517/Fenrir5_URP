using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healAmount = 20;  // �ɦ�]���Ѫ��ͩR��

    void OnTriggerEnter(Collider other)
    {
        // �ˬd�I����H�O�_�O���a
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // �p�G�O���a�A��_�ͩR��
            playerHealth.Heal(healAmount);

            // �R���ɦ�]
            Destroy(gameObject);
        }
    }
}
