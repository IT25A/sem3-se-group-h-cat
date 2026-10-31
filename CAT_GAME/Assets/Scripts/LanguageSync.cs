using UnityEngine;

public class LanguageSync : MonoBehaviour
{
    // 0 = Englisch, 1 = Deutsch, 2 = Spanisch
    public void SetDialogueLanguage(int langID)
    {
        PlayerPrefs.SetInt("Language", langID);
        PlayerPrefs.Save(); // Speichert es sofort fest auf der Festplatte
    }
}