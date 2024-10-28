using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public GameObject portal;  // �ǰe����H
    public int requiredKills = 15;  // �}�Ҷǰe���һݪ�������
    private int killCount = 0;  // ��e������
    private bool canEnterPortal = false;  // �O�_����i�J�ǰe��

    void Start()
    {
        // �ǰe���@�}�l�i�H��ܡA�������\�i�J
        portal.SetActive(true);  // �T�O�ǰe���q�@�}�l�N���
    }

    // �W�[������
    public void AddKill()
    {
        killCount++;
        Debug.Log("������: " + killCount);

        if (killCount >= requiredKills)
        {
            EnablePortalEntry();
        }
    }

    // ���\�i�J�ǰe��
    void EnablePortalEntry()
    {
        Debug.Log("�ǰe���{�b�i�H�i�J�I");
        canEnterPortal = true;  // �������ƹF��n�D�A���\�i�J�ǰe��
    }

    // ���a�i�J�ǰe����Ĳ�o�U�@��
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canEnterPortal)
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
