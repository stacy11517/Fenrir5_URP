using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextBoxManager : MonoBehaviour
{
    public GameObject marqueePanel;
    public GameObject textBoxPanel;
    public GameObject fadingPanel;
    public Image displayText;
    public Sprite[] textSprites;
    public float textDuration = 3.0f;

    private int currentTextIndex = 0;
    private bool isSkipping = false;
    private float holdTime = 0f;
    private bool isHoldingKey = false;

    public Animator fadingAni;
    public Animator marqueeAni;
    private bool texthasShowed = false;
    private bool marqueeGone = false;


    // Start is called before the first frame update
    void Start()
    {
        marqueePanel.SetActive(false);
        fadingPanel.SetActive(false);
        textBoxPanel.SetActive(true);
        texthasShowed = false;

        StartCoroutine("PlayTextBox");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            if (!isHoldingKey)
            {
                isHoldingKey = true;
                holdTime = 0f;
            }

            holdTime = Time.deltaTime;
            if (holdTime >= 3.0f)
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            isHoldingKey = false;
        }
    }

    IEnumerator PlayTextBox()
    {
        while (currentTextIndex < textSprites.Length)
        {
            displayText.sprite = textSprites[currentTextIndex];

            float elapsedTime = 0f;
            while (elapsedTime < textDuration)
            {
                if (isSkipping)
                    yield break;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Debug.Log("123");
            currentTextIndex++;
        }
        FadingBlack();
        marqueeGoUp();
        marqueeGone = true;
        yield return new WaitForSeconds(30.0f);
        BackToMainMenu();

    }

    void FadingBlack()
    {
        fadingPanel.SetActive(true);
        fadingAni.SetTrigger("Fading");
        textBoxPanel.SetActive(false);
        texthasShowed = true;
    }

    void marqueeGoUp()
    {
        marqueePanel.SetActive(true);
        marqueeAni.SetTrigger("StartGoUp");
    }

    void BackToMainMenu()
    {
        if (marqueeGone == true)
        {
            SceneManager.LoadScene(0);
        }
    }
}
