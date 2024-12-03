using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgmClips; // �s��C�����d�� BGM
    private AudioSource audioSource;

    public float fadeDuration = 1.5f; // ���� BGM �ɪ��H�J�H�X�ɶ�

    private static BGMManager instance; // �T�O��ҼҦ�
    private int currentLevelIndex = -1; // ��e�������d����

    void Awake()
    {
        // ��ҼҦ��T�O�ߤ@ BGM �޲z��
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

        // �q�\�����ܧ�ƥ�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // �T�O�����ƥ�q�\
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

    // �b�����[���ɼ�������� BGM
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGM(scene.buildIndex);
    }

    // ����������d�� BGM
    public void PlayBGM(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= bgmClips.Length)
        {
            Debug.LogError("�L�Ī����d���ޡG" + levelIndex);
            return;
        }

        // �p�G���b����P�@�� BGM�A������^
        if (currentLevelIndex == levelIndex)
            return;

        currentLevelIndex = levelIndex;
        StartCoroutine(SwitchBGM(bgmClips[levelIndex]));
    }

    // �H�J�H�X���� BGM
    private IEnumerator SwitchBGM(AudioClip newClip)
    {
        // �H�X��e BGM
        if (audioSource.isPlaying)
        {
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(1f, 0f, t / fadeDuration);
                yield return null;
            }
            audioSource.Stop();
        }

        // ������s BGM
        audioSource.clip = newClip;
        audioSource.Play();

        // �H�J�s BGM
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        audioSource.volume = 1f;
    }
}
