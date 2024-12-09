using UnityEngine;

public class EnemyDeactivationTrigger : MonoBehaviour
{
    public GameObject enemySpawner;  // 敌人生成器
    public string enemyTag = "Enemy";  // 地图上敌人使用的标签
    public string playerTag = "Player"; // 玩家对象的标签

    private void OnTriggerEnter(Collider other)
    {
        // 检测是否是玩家碰撞触发器
        if (other.CompareTag(playerTag))
        {
            // 关闭敌人生成器
            if (enemySpawner != null)
            {
                enemySpawner.SetActive(false); // 停止敌人生成器
            }

            // 删除所有敌人
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy); // 删除敌人对象
            }

            // 销毁触发器自身（可选）
            Destroy(gameObject);
        }
    }
}
