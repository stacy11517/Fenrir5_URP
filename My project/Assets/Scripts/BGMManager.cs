using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgmClips; // �s��C�����d�� BGM
    public AudioSource audioSource;
    public float fadeDuration = 1.5f; // �H�J�H�X�ɶ�

    private static BGMManager instance; // ��ҼҦ�
    private int currentLevelIndex = -1; // ��e�������d����

    void Awake()
    {
        // �T�O�ߤ@ BGM �޲z��
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

        // �q�\�����ܧ�ƥ�
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
                Debug.LogError("BGMManager �|����l�ơI");
            }
            return instance;
        }
    }

    // ����I������
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGM(scene.buildIndex);
    }

    public void PlayBGM(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= bgmClips.Length)
        {
            Debug.LogError("�L�Ī����d���ޡG" + levelIndex);
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

    // ���\ UIbutton �}���ե� AudioSource
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }
}
