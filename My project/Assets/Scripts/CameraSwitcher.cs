using System.Collections;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera introCamera;  // ��l����v���A�Ω�������d�D�D
    public Camera mainCamera;   // �C�����D��v��
    public float introDuration = 5f;  // ����D�D������ɶ��]��^

    void Start()
    {
        // �T�O�}�l�� introCamera �ҰʡAmainCamera ����
        mainCamera.gameObject.SetActive(false);
        introCamera.gameObject.SetActive(true);

        // �b���w�ɶ��������D��v��
        StartCoroutine(SwitchToMainCamera());
    }

    IEnumerator SwitchToMainCamera()
    {
        // ���ݫ��w���ɶ�
        yield return new WaitForSeconds(introDuration);

        // ������v��
        introCamera.gameObject.SetActive(false);  // �����D�D��v��
        mainCamera.gameObject.SetActive(true);    // �ҰʥD��v��
    }
}
