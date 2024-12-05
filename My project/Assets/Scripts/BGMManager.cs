using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgmClips; // 存放每個關卡的 BGM
    public AudioSource audioSource;
    public float fadeDuration = 1.5f; // 淡入淡出時間

    private static BGMManager instance; // 單例模式
    private int currentLevelIndex = -1; // 當前播放的關卡索引

    void Awake()
    {
        // 確保唯一 BGM 管理器
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // 訂閱場景變更事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
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

    // 播放背景音樂
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGM(scene.buildIndex);
    }

    public void PlayBGM(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= bgmClips.Length)
        {
            Debug.LogError("無效的關卡索引：" + levelIndex);
            return;
        }

        if (currentLevelIndex == levelIndex)
            return;

        currentLevelIndex = levelIndex;
        StartCoroutine(SwitchBGM(bgmClips[levelIndex]));
    }

    private IEnumerator SwitchBGM(AudioClip newClip)
    {
        if (audioSource.isPlaying)
        {
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(1f, 0f, t / fadeDuration);
                yield return null;
            }
            audioSource.Stop();
        }

        audioSource.clip = newClip;
        audioSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 1f;
    }

    // 允許 UIbutton 腳本調用 AudioSource
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }
}
