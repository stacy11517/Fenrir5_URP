using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    //�t�d��v�����H���a�A�ä䴩����\��C
    public Transform player;
    public float followSpeed = 5f;
    public Vector3 offset;
    private bool isSideView = false;

    void LateUpdate()
    {
        if (player == null) return;

        if (isSideView)
        {
            Vector3 sidePosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, sidePosition, followSpeed * Time.deltaTime);
            transform.LookAt(player.position);
        }
        else
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    public void SetSideView()
    {
        isSideView = true;
    }

    public void ResetView()
    {
        isSideView = false;
    }
}
