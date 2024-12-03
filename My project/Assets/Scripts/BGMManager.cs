using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgmClips; // 存放每個關卡的 BGM
    private AudioSource audioSource;

    public float fadeDuration = 1.5f; // 切換 BGM 時的淡入淡出時間

    private static BGMManager instance; // 確保單例模式
    private int currentLevelIndex = -1; // 當前播放的關卡索引

    void Awake()
    {
        // 單例模式確保唯一 BGM 管理器
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = true;

        // 訂閱場景變更事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // 確保取消事件訂閱
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static BGMManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("BGMManager 尚未初始化！");
            }
            return instance;
        }
    }

    // 在場景加載時播放對應的 BGM
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGM(scene.buildIndex);
    }

    // 播放對應關卡的 BGM
    public void PlayBGM(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= bgmClips.Length)
        {
            Debug.LogError("無效的關卡索引：" + levelIndex);
            return;
        }

        // 如果正在播放同一首 BGM，直接返回
        if (currentLevelIndex == levelIndex)
            return;

        currentLevelIndex = levelIndex;
        StartCoroutine(SwitchBGM(bgmClips[levelIndex]));
    }

    // 淡入淡出切換 BGM
    private IEnumerator SwitchBGM(AudioClip newClip)
    {
        // 淡出當前 BGM
        if (audioSource.isPlaying)
        {
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(1f, 0f, t / fadeDuration);
                yield return null;
            }
            audioSource.Stop();
        }

        // 切換到新 BGM
        audioSource.clip = newClip;
        audioSource.Play();

        // 淡入新 BGM
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 1f;
    }
}
