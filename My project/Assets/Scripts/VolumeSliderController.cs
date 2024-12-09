using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        BackgroundMusicManager bgmManager = FindObjectOfType<BackgroundMusicManager>();

        if (bgmManager != null)
        {
            // 初始化 Slider 值，如果没有设置默认为 1
            volumeSlider.value = bgmManager.GetVolume();

            // 同步 Slider 值变化
            volumeSlider.onValueChanged.AddListener(bgmManager.SetVolume);
        }
    }
}
