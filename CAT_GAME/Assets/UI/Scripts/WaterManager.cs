using UnityEngine;
using UnityEngine.UI;

public class WaterManager : MonoBehaviour {
    [Header("UI References")]
    public Slider waterSlider;      
    public GameObject gameOverUI;    

    [Header("Settings")]
    public float maxWater = 100f;
    public static float currentWater = -1f;
    public float decreaseSpeed = 5f; 

    [Header("Notification")]
    public float warningThreshold = 30f; 
    private bool hasWarned = false;     

    [Header("Localization Keys")]
    public string tableName = "UI_TextTable";
    public string warningKey = "water_low_warning";
    public string newWaterKey = "water_refilled";

    public bool isGameOver = false;
    public bool isGameStarted = false; 

    void Start() {
        if (SceneData.savedWater < 0) {
            SceneData.savedWater = maxWater;
        }

        if (waterSlider != null) {
            waterSlider.maxValue = maxWater;
            waterSlider.value = SceneData.savedWater;
        }

        if (gameOverUI != null) {
            gameOverUI.SetActive(false);
        }
    }

    private void Update() {
        if (isGameOver || !isGameStarted || MenuManager.currentState != MenuManager.GameState.Playing) return;

        if (SceneData.savedWater > 0) {
            SceneData.savedWater -= decreaseSpeed * Time.deltaTime;
            SceneData.savedWater = Mathf.Max(SceneData.savedWater, 0); 

            if (waterSlider != null) {
                waterSlider.value = SceneData.savedWater;
            }

            if (SceneData.savedWater <= warningThreshold && !hasWarned) {
                hasWarned = true;
                if (NotificationManager.Instance != null) {
                    NotificationManager.Instance.ShowLocalizedNotification(tableName, warningKey);
                }
            }
        }
        else {
            TriggerGameOver();
        }
    }

    public void StartWaterDecrease() {
        isGameStarted = true;
    }

    private void TriggerGameOver() {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverUI != null) {
            gameOverUI.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void RefillWater(float amount) {
        SceneData.savedWater += amount;
        SceneData.savedWater = Mathf.Min(SceneData.savedWater, maxWater);

        if (SceneData.savedWater > warningThreshold) {
            hasWarned = false;
        }
    }
}