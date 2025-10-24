using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SoundSettingsSliders : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TMP_Text musicValueText;
    [SerializeField] private TMP_Text sfxValueText;

    private void Start()
    {
        if (SoundManager.instance == null)
        {
            Debug.LogError("SoundManager instance not found in scene!");
            return;
        }

        float savedMusic = Mathf.Clamp(PlayerPrefs.GetFloat("MusicVolume", 100f), 0f, 100f);
        float savedSfx = Mathf.Clamp(PlayerPrefs.GetFloat("SfxVolume", 100f), 0f, 100f);

        SoundManager.instance.SetMusicVolume(savedMusic / 100f);
        SoundManager.instance.SetSfxBaseVolume(savedSfx / 100f);

        if (musicSlider)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 100f;
            musicSlider.value = savedMusic;
        }

        if (sfxSlider)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 100f;
            sfxSlider.value = savedSfx;
        }

        UpdateValueTexts();

        if (musicSlider) musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        if (sfxSlider) sfxSlider.onValueChanged.AddListener(OnSfxSliderChanged);
    }

    private void OnMusicSliderChanged(float value)
    {
        value = Mathf.Clamp(value, 0f, 100f);
        SoundManager.instance.SetMusicVolume(value / 100f);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        UpdateValueTexts();
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnSfxSliderChanged(float value)
    {
        value = Mathf.Clamp(value, 0f, 100f);
        SoundManager.instance.SetSfxBaseVolume(value / 100f);
        PlayerPrefs.SetFloat("SfxVolume", value);
        PlayerPrefs.Save();
        UpdateValueTexts();
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void UpdateValueTexts()
    {
        if (musicValueText) musicValueText.text = "Music: " + Mathf.RoundToInt(musicSlider.value) + "%";
        if (sfxValueText) sfxValueText.text = "SFX: " + Mathf.RoundToInt(sfxSlider.value) + "%";
    }
}







