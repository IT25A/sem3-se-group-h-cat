using UnityEngine;
using UnityEngine.InputSystem; // Wichtig: Bindet das neue Input System ein!

public class WaterRefillTrigger : MonoBehaviour {
    [Header("Refill Settings")]
    public float refillAmount = 50f; 
    private bool playerIsNearby = false;

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerIsNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerIsNearby = false;
            Debug.Log("Spieler hat den Fluss verlassen.");
        }
    }

    private void Update() {
        // NEU: Abfrage für das neue Input System (prüft, ob das Keyboard existiert und E gedrückt wurde)
        if (playerIsNearby && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) {
            RefillPlayerWater();
        }
    }

    private void RefillPlayerWater() {
        WaterManager waterManager = FindAnyObjectByType<WaterManager>();
        if (waterManager != null) {
            waterManager.RefillWater(refillAmount);
            Debug.Log("Wasser am Fluss aufgefüllt!");
        }
    }
}