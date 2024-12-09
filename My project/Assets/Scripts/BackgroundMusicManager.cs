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
            audioSource.volume = globalVolume; // 初始音量设置
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
        float startVolume = audioSource.volume;

        // 淡出当前音乐
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = 0;

        // 切换到新音乐
        audioSource.clip = newClip;
        if (newClip != null)
        {
            audioSource.Play();

            // 淡入新音乐
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(0, globalVolume, t / fadeDuration);
                yield return null;
            }
            audioSource.volume = globalVolume; // 确保最终音量正确
        }
    }
    // 设置音量（Slider 调用此方法）
    public void SetVolume(float volume)
    {
        globalVolume = Mathf.Clamp01(volume);
        audioSource.volume = globalVolume; // 即时更新音量
    }
    // 获取当前音量（方便 UI 初始化时同步显示）
    public float GetVolume()
    {
        return globalVolume;
    }

}
