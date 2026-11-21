using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class FoodManager : MonoBehaviour {

    public static FoodManager Instance { get; private set; }

    [Header("UI Reference")]
    public TextMeshProUGUI foodText; 

    [Header("Localization Keys")]
    public string tableName = "MenuText";
    public string foodReceivedKey = "foodCount";

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

        if (NotificationManager.Instance != null) {
            NotificationManager.Instance.ShowLocalizedNotification(tableName, foodReceivedKey);
        }
    }

    private void UpdateFoodUI() {
        if (foodText != null) {
            foodText.text = SceneData.savedFood.ToString("D3");
        }
    }
}