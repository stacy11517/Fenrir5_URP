using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public GameObject mainMenuPanel;   // �D��檺 Panel
    public GameObject cutscenePanel;  // �Ω���ܹϤ��� Panel
    public Image displayImage;        // ��ܹϤ��� UI Image
    public Sprite[] cutsceneSprites;  // �������Ϥ��}�C
    public float imageDuration = 3f;  // �C�i�Ϥ���ܮɶ�

    private int currentImageIndex = 0; // ��e���񪺹Ϥ�����
    private bool isSkipping = false;   // �O�_���b���L
    private float holdTime = 0f;       // �������ɶ�
    private bool isHoldingKey = false; // �O�_���b������

    void Start()
    {
        // ��l�ơG�T�O�D�����ܡA�������O����
        mainMenuPanel.SetActive(true);
        cutscenePanel.SetActive(false);
    }

    void Update()
    {
        // �˴��O�_�������N��
        if (Input.anyKey)
        {
            if (!isHoldingKey)
            {
                isHoldingKey = true;
                holdTime = 0f; // ��l�ƫ���ɶ�
            }

            holdTime += Time.deltaTime;
            if (holdTime >= 3f) // �p�G����ɶ��F�� 3 ��
            {
                SkipCutscene();
            }
        }
        else
        {
            isHoldingKey = false;
        }
    }

    public void StartGame()
    {
        // �����D��歱�O�A���}�Ϥ����񭱪O
        mainMenuPanel.SetActive(false);
        cutscenePanel.SetActive(true);

        // �}�l��������Ϥ�����{
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        while (currentImageIndex < cutsceneSprites.Length)
        {
            // ��ܷ�e�Ϥ�
            displayImage.sprite = cutsceneSprites[currentImageIndex];

            // ���ݫ��w�ɶ��Ϊ�����L
            float elapsedTime = 0f;
            while (elapsedTime < imageDuration)
            {
                if (isSkipping)
                    yield break; // �p�G���L�A��������
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // ������U�@�i�Ϥ�
            currentImageIndex++;
        }

        // ���񧹲���i�J�Ĥ@��
        LoadFirstLevel();
    }

    void SkipCutscene()
    {
        isSkipping = true; // �]�m���L�аO
        LoadFirstLevel();
    }

    void LoadFirstLevel()
    {
        // ���J�Ĥ@���A���� "Level1" ���A���Ĥ@�������W��
        SceneManager.LoadScene(1);
    }
}
