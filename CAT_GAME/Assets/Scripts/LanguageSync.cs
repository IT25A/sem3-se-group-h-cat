using UnityEngine;

public class LanguageSync : MonoBehaviour
{
    // Speichert die gewählte Dialogsprache dauerhaft (0 = Englisch, 1 = Deutsch, 2 = Spanisch)
    public void SetDialogueLanguage(int langID)
    {
        PlayerPrefs.SetInt("Language", langID);
        PlayerPrefs.Save();
    }
}