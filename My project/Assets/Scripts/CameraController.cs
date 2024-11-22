using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode { FollowPlayer, SideView, FixedView }
    public CameraMode currentMode;

    public Transform target;         // ���a���⪺ Transform
    public Vector3 offset;           // �۾��P���a�������q
    public Transform fixedPosition;  // �T�w��v����m�]�ĤT���^

    public float shakeDuration = 0.5f;   // �_�ʫ���ɶ�
    public float shakeMagnitude = 0.1f; // �_�ʴT��

    private bool isShaking = false;  // �O�_���b�_��

    void Start()
    {
        // �p�G offset ���]�m�A�p���l�����q
        if (offset == Vector3.zero && target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        if (isShaking) return; // �p�G���b�_�ʡA���L���q��s

        UpdateCameraPosition(); // ��s��v����m
    }

    /// <summary>
    /// ��s��v����m�M�欰
    /// </summary>
    void UpdateCameraPosition()
    {
        switch (currentMode)
        {
            case CameraMode.FollowPlayer:
                FollowPlayer();
                break;

            case CameraMode.SideView:
                SideView();
                break;

            case CameraMode.FixedView:
                FixedView();
                break;
        }
    }

    /// <summary>
    /// ��v�����H���a
    /// </summary>
    void FollowPlayer()
    {
        if (target == null) return;
        transform.position = target.position + offset;
        transform.LookAt(target);
    }

    /// <summary>
    /// ��v�����窱�a
    /// </summary>
    void SideView()
    {
        if (target == null) return;

        Vector3 sideOffset = offset;
        sideOffset.z = 0; // ��w Z �b�A��{�����ĪG
        transform.position = target.position + sideOffset;
        transform.LookAt(target);
    }

    /// <summary>
    /// ��v���T�w��m
    /// </summary>
    void FixedView()
    {
        if (fixedPosition == null) return;
        transform.position = fixedPosition.position;
        transform.rotation = fixedPosition.rotation;
    }

    /// <summary>
    /// �]�w��v���Ҧ�
    /// </summary>
    /// <param name="mode">�s����v���Ҧ�</param>
    public void SetCameraMode(CameraMode mode)
    {
        currentMode = mode;
    }

    /// <summary>
    /// Ĳ�o��v���_��
    /// </summary>
    /// <param name="duration">�_�ʫ���ɶ�</param>
    /// <param name="magnitude">�_�ʴT��</param>
    public void TriggerShake(float duration, float magnitude)
    {
        if (!isShaking)
        {
            shakeDuration = duration;
            shakeMagnitude = magnitude;
            StartCoroutine(Shake());
        }
    }

    /// <summary>
    /// ��v���_�ʨ�{
    /// </summary>
    IEnumerator Shake()
    {
        isShaking = true;

        Vector3 originalPosition = transform.position; // �O���_�ʫe����l��m
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            Vector3 shakePosition = new Vector3(x, y, 0); // �_�ʦ�m����
            transform.position = target.position + offset + shakePosition; // ��s��v����m

            elapsed += Time.deltaTime;

            yield return null;
        }

        // ��_�쥿�`��m
        transform.position = target.position + offset;
        isShaking = false;
    }
}
