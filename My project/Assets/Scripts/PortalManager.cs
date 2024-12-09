using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public int requiredKills = 15;          // �}�Ҷǰe���һݪ�������
    private int killCount = 0;              // ��e������
    private bool canEnterPortal = false;    // �O�_����i�J�ǰe��
    public string nextSceneName;            // �U�@���d���W�١]�i��^

    public LevelLoader levelLoader;

    void Start()
    {
        canEnterPortal = false;
        killCount = 0;
    }


    /// �W�[������
    public void AddKill()
    {
        killCount++;
        Debug.Log("������: " + killCount);

        if (killCount >= requiredKills)
        {
            Debug.Log("�ǰe���{�b�i�H�i�J�I");
            canEnterPortal = true;
        }
    
    }

    /// <summary>
    /// ���a�i�J�ǰe�����d���Ĳ�o
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canEnterPortal==true)
        {
            Debug.Log("�}�Ҷǰe���A�i�J�U�@���I");
            levelLoader.LoadNextLevel();
        }
    }

}
