using System.Collections;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public CameraController cameraController; // �s����v�����
    public CameraController.CameraMode sideViewMode = CameraController.CameraMode.SideView; // ����Ҧ�
    public CameraController.CameraMode followMode = CameraController.CameraMode.FollowPlayer; // ���H�Ҧ�

    public Transform platform; // �S�w�ϰ쪺�a������
    public Transform nextPlatform; // �U�@���a��
    public float[] skillOffsets = { -3f, -6f, -9f }; // �C���ޯ�ϥΫᥭ�x�U�����Z��
    public float moveDuration = 0.5f; // ���x���ʩһݮɶ�
    private int skillUseCount = 0; // �ޯ�ϥΦ���
    private bool playerInZone = false; // ���a�O�_�b�ϰ줺

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true; // ���a�i�J�ϰ�
            cameraController.SetCameraMode(sideViewMode); // �����찼��Ҧ�
            Debug.Log("Player entered zone. Camera switched to SideView.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false; // ���a���}�ϰ�
            cameraController.SetCameraMode(followMode); // �����^���H�Ҧ�
            Debug.Log("Player exited zone. Camera switched to FollowPlayer.");
        }
    }

    public void RegisterSkillUse()
    {
        if (!playerInZone || skillUseCount >= skillOffsets.Length) return;

        // �W�[�ޯ�ϥΦ���
        float offset = skillOffsets[skillUseCount];
        skillUseCount++;

        // ���Ʋ��ʥ��x
        if (platform != null)
        {
            StartCoroutine(SmoothMovePlatform(platform.position + new Vector3(0, offset, 0), moveDuration));
        }

        // �p�G�O�̫�@���ޯ�ϥΡA�s���U�@���a��
        if (skillUseCount == skillOffsets.Length && nextPlatform != null)
        {
            Vector3 newPosition = platform.position;
            newPosition.y = nextPlatform.position.y; // �T�O�s��m���W�U�@���a��
            StartCoroutine(SmoothMovePlatform(newPosition, moveDuration));
            Debug.Log("Platform connected to next platform.");
        }
    }

    private IEnumerator SmoothMovePlatform(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = platform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            platform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        platform.position = targetPosition;
        Debug.Log("Platform moved to position: " + targetPosition);
    }
}
