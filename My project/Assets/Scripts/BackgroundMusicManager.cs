using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BackgroundMusicManager : MonoBehaviour
{
    private static BackgroundMusicManager instance;

    public AudioClip[] sceneClips;      // 每个场景对应的音乐
    public float fadeDuration = 1.0f;   // 淡入淡出的时间

    [Range(0f, 1f)] public float globalVolume = 1.0f; // 全局音量

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 保持场景切换时不销毁
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject); // 如果已有实例，销毁新对象
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneIndex = scene.buildIndex; // 获取场景索引

        // 如果当前场景有对应的音频资源，播放对应音效
        if (sceneIndex < sceneClips.Length && sceneClips[sceneIndex] != null)
        {
            ChangeMusic(sceneClips[sceneIndex]);
        }
        else
        {
            audioSource.Stop(); // 如果没有对应音效，停止播放
        }
    }

    public void ChangeMusic(AudioClip newClip)
    {
        StartCoroutine(FadeOutAndChange(newClip));
    }

    private IEnumerator FadeOutAndChange(AudioClip newClip)
    {
        if (audioSource.isPlaying)
        {
            // 淡出当前音乐
            while (audioSource.volume > 0.1f)
            {
                audioSource.volume -= Time.deltaTime / fadeDuration;
                yield return null;
            }
            audioSource.Stop();
        }

        // 切换到新音乐
        audioSource.clip = newClip;
        if (newClip != null)
        {
            audioSource.Play();

            // 淡入新音乐
            while (audioSource.volume < globalVolume)
            {
                audioSource.volume += Time.deltaTime / fadeDuration;
                yield return null;
            }
            audioSource.volume = globalVolume; // 确保最终音量正确
        }
    }
}
