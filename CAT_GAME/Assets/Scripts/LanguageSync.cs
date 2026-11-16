using UnityEngine;

public class LanguageSync : MonoBehaviour
{
    // Permanently saves the selected dialogue language (0 = English, 1 = German, 2 = Spanish)
    public void SetDialogueLanguage(int langID)
    {
        PlayerPrefs.SetInt("Language", langID);
        PlayerPrefs.Save();
    }
}