using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIbutton : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Game is quitting...");

        // �b�s�边�Ҧ��U�|�����
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // �b build �X�Ӫ��C�����|�������ε{��
        Application.Quit();
#endif
    }
}
