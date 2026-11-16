using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour {
    private bool isChangingLanguage = false;
    public void ChangeLanguage(int localeID) {
        if (!isChangingLanguage) {
            StartCoroutine(SetLocale(localeID));
        }
    }

    // Waits for the Unity Localization system to initialize and then switches the active locale
    private IEnumerator SetLocale(int localeID) {
        isChangingLanguage = true;
        yield return LocalizationSettings.InitializationOperation;
        yield return null;
        if (localeID < LocalizationSettings.AvailableLocales.Locales.Count) {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        }
        isChangingLanguage = false;
    }
}