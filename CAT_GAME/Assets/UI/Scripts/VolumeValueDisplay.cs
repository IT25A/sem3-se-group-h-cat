using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class VolumeValueDisplay : MonoBehaviour
{
    [Header("Music-Settings")]
    public AudioMixer audioMixer;
    public Slider sliderMusic;
    public TMP_Text valueTextMusic;
    public string musicKey = "MusicVolume";
    public float defaultValueMusic = 75f;

    [Header("Sound-Settings")]
    public Slider sliderSound;
    public TMP_Text valueTextSound;
    public string soundKey = "SoundVolume";
    public float defaultValueSound = 75f;

    private void OnEnable() {
        float savedMusic = PlayerPrefs.GetFloat(musicKey, defaultValueMusic);
        if (sliderMusic != null) {
            sliderMusic.SetValueWithoutNotify(savedMusic);
            sliderMusic.onValueChanged.AddListener(OnMusicSliderChanged);
        }
        UpdateMusicText(savedMusic);

        float savedSound = PlayerPrefs.GetFloat(soundKey, defaultValueSound);
        if (sliderSound != null) {
            sliderSound.SetValueWithoutNotify(savedSound);
            sliderSound.onValueChanged.AddListener(OnSoundSliderChanged);
        }
        UpdateSoundText(savedSound);
    }

    private void OnDisable() {
        if (sliderMusic != null) {
            sliderMusic.onValueChanged.RemoveListener(OnMusicSliderChanged);
        }
        if (sliderSound != null) {
            sliderSound.onValueChanged.RemoveListener(OnSoundSliderChanged);
        }
    }

    private void SetMixerVolume(string exposedParam, float sliderValue)
    {
        if (audioMixer == null) return;

        // Clamp percentage (0-100) to 0.0001-1.0 to avoid log10(0) and convert to decibels (-80 dB to 0 dB)
        float linear = Mathf.Clamp(sliderValue / 100f, 0.0001f, 1f);
        float dB = Mathf.Log10(linear) * 20f;

        if (sliderValue <= 0f)
            dB = -80f; // Complete silence at 0%

        audioMixer.SetFloat(exposedParam, dB);
    }    

    private void OnMusicSliderChanged(float newValue) {
        PlayerPrefs.SetFloat(musicKey, newValue);
        PlayerPrefs.Save();
        UpdateMusicText(newValue);
        SetMixerVolume("MusicVolume", newValue);    
    }

    private void OnSoundSliderChanged(float newValue) {
        PlayerPrefs.SetFloat(soundKey, newValue);
        PlayerPrefs.Save();
        UpdateSoundText(newValue);
        SetMixerVolume("SoundVolume", newValue);
    }

    private void UpdateMusicText(float value) {
        if (valueTextMusic != null) {
            valueTextMusic.text = Mathf.RoundToInt(value).ToString();
        }
    }

    private void UpdateSoundText(float value) {
        if (valueTextSound != null) {
            valueTextSound.text = Mathf.RoundToInt(value).ToString();
        }
    }

    public void ResetAllToDefault() {
        PlayerPrefs.SetFloat(musicKey, defaultValueMusic);
        PlayerPrefs.SetFloat(soundKey, defaultValueSound);
        PlayerPrefs.Save();

        if (sliderMusic != null) {
            sliderMusic.value = defaultValueMusic;
        }
        UpdateMusicText(defaultValueMusic);

        if (sliderSound != null) {
            sliderSound.value = defaultValueSound;
        }
        UpdateSoundText(defaultValueSound);
    }
}