using UnityEngine;

public class ThunderBall : MonoBehaviour
{
    public int damage = 50; // �p�y���ˮ`

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player hit by Thunder Ball! Took " + damage + " damage.");
            }
        }

        // �p�G�I��a�ΡA�P���p�y
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
