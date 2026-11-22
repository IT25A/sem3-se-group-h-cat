using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

// After every scene load: applies the saved (default 75%) volume to the audio mixer, even if the
// settings panel (VolumeValueDisplay) is inactive, and starts the scene's "Music" source once the game runs.
public static class SceneMusicStarter
{
    private const string IntroSceneName = "Introscene";
    private const float DefaultVolume = 75f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject musicObj = GameObject.Find("Music");
        AudioSource source = musicObj != null ? musicObj.GetComponent<AudioSource>() : null;

        if (source != null && source.outputAudioMixerGroup != null)
        {
            AudioMixer mixer = source.outputAudioMixerGroup.audioMixer;
            ApplyVolume(mixer, "MusicVolume", PlayerPrefs.GetFloat("MusicVolume", DefaultVolume));
            ApplyVolume(mixer, "SoundVolume", PlayerPrefs.GetFloat("SoundVolume", DefaultVolume));
        }

        if (!SceneData.isGameStarted || scene.name == IntroSceneName) return;
        if (source != null && !source.isPlaying) source.Play();
    }

    // Percentage (0-100) -> decibels, same formula as VolumeValueDisplay
    private static void ApplyVolume(AudioMixer mixer, string param, float percent)
    {
        float linear = Mathf.Clamp(percent / 100f, 0.0001f, 1f);
        mixer.SetFloat(param, percent <= 0f ? -80f : Mathf.Log10(linear) * 20f);
    }
}

