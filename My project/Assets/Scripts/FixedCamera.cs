using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    //�T�w��v����m�M���סA�Ω�ĤT���C
    public void SetFixedPosition(Transform fixedPosition)
    {
        if (fixedPosition != null)
        {
            transform.position = fixedPosition.position;
            transform.rotation = fixedPosition.rotation;
        }
    }
}
