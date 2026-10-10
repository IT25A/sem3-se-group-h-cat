using UnityEngine;
using UnityEngine.UI;

public class WaterManager : MonoBehaviour
{
    [Header("UI References")]
    public Slider waterSlider;      
    public GameObject gameOverUI;    

    [Header("Settings")]
    public float maxWater = 100f;
    public float currentWater;
    public float decreaseSpeed = 5f; 

    [Header("Notification")]
    public float warningThreshold = 30f; 
    private bool hasWarned = false;     

    [Header("Localization Keys")]
    public string tableName = "UI_TextTable";
    public string warningKey = "water_low_warning";

    public bool isGameOver = false;
    public bool isGameStarted = false; 

    private void Start() {
        currentWater = maxWater;

        if (waterSlider != null) {
            waterSlider.maxValue = maxWater;
            waterSlider.value = currentWater;
        }

        if (gameOverUI != null) {
            gameOverUI.SetActive(false);
        }
    }

    private void Update() {
        if (isGameOver || !isGameStarted) return;

        if (currentWater > 0) {
            currentWater -= decreaseSpeed * Time.deltaTime;
            currentWater = Mathf.Max(currentWater, 0); 

            if (waterSlider != null) {
                waterSlider.value = currentWater;
            }

            if (currentWater <= warningThreshold && !hasWarned) {
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

        if (gameOverUI != null) {
            gameOverUI.SetActive(true);
        }
    }

    public void RefillWater(float amount) {
        currentWater += amount;
        currentWater = Mathf.Min(currentWater, maxWater);

        if (currentWater > warningThreshold) {
            hasWarned = false;
        }
    }
}