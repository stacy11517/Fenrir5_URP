using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIbutton : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Game is quitting...");

        // 在編輯器模式下會停止播放
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在 build 出來的遊戲中會關閉應用程式
        Application.Quit();
#endif
    }
}
