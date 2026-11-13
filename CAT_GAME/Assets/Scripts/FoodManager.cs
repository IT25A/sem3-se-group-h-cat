using UnityEngine;
using TMPro;
using Unity.VisualScripting;

// Verwaltet den globalen Futterbestand, die UI-Anzeige und Benachrichtigungen beim Erhalt von Futter
public class FoodManager : MonoBehaviour {

    public static FoodManager Instance { get; private set; }

    [Header("UI Reference")]
    public TextMeshProUGUI foodText; 

    [Header("Localization Keys")]
    public string tableName = "MenuText";
    public string foodReceivedKey = "foodCount";
    public string hundredfoodKey = "100food";

    private void Awake() {
        if (Instance == null) {
            Instance = this; 
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        UpdateFoodUI();
    }

    public void AddFood(int amount) {
        SceneData.savedFood += amount;
        UpdateFoodUI();

        // Spezielle Benachrichtigung bei 100 Futter, ansonsten Standard-Meldung anzeigen
        if (NotificationManager.Instance != null) {
            if (SceneData.savedFood == 100) {
                NotificationManager.Instance.ShowLocalizedNotification(tableName, hundredfoodKey);
            } else {
                NotificationManager.Instance.ShowLocalizedNotification(tableName, foodReceivedKey);

            }
        }
    }

    private void UpdateFoodUI() {
        if (foodText != null) {
            foodText.text = SceneData.savedFood.ToString("D3");
        }
    }
}