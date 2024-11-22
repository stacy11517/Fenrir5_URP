using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    //固定攝影機位置和角度，用於第三關。
    public void SetFixedPosition(Transform fixedPosition)
    {
        if (fixedPosition != null)
        {
            transform.position = fixedPosition.position;
            transform.rotation = fixedPosition.rotation;
        }
    }
}
