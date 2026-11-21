using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSync : MonoBehaviour
{
    // Permanently saves the selected dialogue language (0 = English, 1 = German, 2 = Spanish)
    private void Start()
    {
        LoadSavedLanguage();
    }
    public void SetDialogueLanguage(int langID)
    {
        PlayerPrefs.SetInt("Language", langID);
        PlayerPrefs.Save();

        ChangeLocale(langID);
    }

    private void ChangeLocale(int langID)
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;

        if (langID >= 0 && langID < locales.Count)
        {
            LocalizationSettings.SelectedLocale = locales[langID];
        }
    }

    private void LoadSavedLanguage()
    {
        int savedLang = PlayerPrefs.GetInt("Language", 1); 
        ChangeLocale(savedLang);
    }
}