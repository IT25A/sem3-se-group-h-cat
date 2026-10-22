using UnityEngine;
using UnityEngine.InputSystem;

public class WaterRefillTrigger : MonoBehaviour {
    [Header("Refill Settings")]
    public float refillAmount = 50f;
    [Header("Localization Keys")]
    public string tableName = "UI_TextTable";
    public string waterRefilled = "waterRefill";
    
    private GameObject currentStaticPlayer = null;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            currentStaticPlayer = collision.gameObject;
            Debug.Log("Am Fluss! Drücke E zum Auffüllen.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            currentStaticPlayer = null;
            Debug.Log("Fluss verlassen.");
        }
    }

    private void Update() {
        if (currentStaticPlayer != null && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) {
            RefillPlayerWater();
        }
    }

    private void RefillPlayerWater() {
        WaterManager waterManager = FindAnyObjectByType<WaterManager>();
        if (waterManager != null) {
            waterManager.RefillWater(refillAmount);
        }
        if (NotificationManager.Instance != null) {
            NotificationManager.Instance.ShowLocalizedNotification(tableName, waterRefilled);
        }
    }
}