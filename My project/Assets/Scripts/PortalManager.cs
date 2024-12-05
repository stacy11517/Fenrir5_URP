using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public GameObject portal;                // �ǰe����H
    public int requiredKills = 15;          // �}�Ҷǰe���һݪ�������
    private int killCount = 0;              // ��e������
    private bool canEnterPortal = false;    // �O�_����i�J�ǰe��
    public string nextSceneName;            // �U�@���d���W�١]�i��^

    void Start()
    {
        // ��l�ƶǰe�����A
        if (portal != null)
        {
            portal.SetActive(false);        // �ǰe���b�@�}�l�O���ê�
        }
    }

    /// <summary>
    /// �W�[������
    /// </summary>
    public void AddKill()
    {
        killCount++;
        Debug.Log("������: " + killCount);

        if (killCount >= requiredKills && !canEnterPortal)
        {
            EnablePortalEntry();
        }
    }

    /// <summary>
    /// ���\�i�J�ǰe��
    /// </summary>
    void EnablePortalEntry()
    {
        Debug.Log("�ǰe���{�b�i�H�i�J�I");
        canEnterPortal = true;

        if (portal != null)
        {
            portal.SetActive(true); // ��ܶǰe��
        }
    }

    /// <summary>
    /// ���a�i�J�ǰe�����d���Ĳ�o
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canEnterPortal)
        {
            Debug.Log("�i�J�ǰe���A�i�J�U�@���I");
            LoadNextLevel();
        }
    }

    /// <summary>
    /// �[���U�@�ӳ���
    /// </summary>
    void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); // �ϥγ����W�٥[��
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // �[���U�@�ӳ���
        }
    }
}
