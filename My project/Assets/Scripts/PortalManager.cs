using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public GameObject portal;  // �ǰe����H
    public int requiredKills = 15;  // �}�Ҷǰe���һݪ�������
    private int killCount = 0;  // ��e������

    void Start()
    {
        portal.SetActive(false);  // �_�l�����öǰe��
    }

    // �W�[������
    public void AddKill()
    {
        killCount++;
        Debug.Log("������: " + killCount);

        if (killCount >= requiredKills)
        {
            OpenPortal();
        }
    }

    // �}�Ҷǰe��
    void OpenPortal()
    {
        Debug.Log("�ǰe���w�}�ҡI");
        portal.SetActive(true);  // ��ܶǰe��
    }

    // ���a�i�J�ǰe����Ĳ�o�U�@��
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && portal.activeSelf)
        {
            Debug.Log("�i�J�ǰe���A�i�J�U�@���I");
            LoadNextLevel();
        }
    }

    // �[���U�@�ӳ���
    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
