using UnityEngine;

public class ThunderBall : MonoBehaviour
{
    public int damage = 50; // 雷球的傷害

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

        // 如果碰到地形，銷毀雷球
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
