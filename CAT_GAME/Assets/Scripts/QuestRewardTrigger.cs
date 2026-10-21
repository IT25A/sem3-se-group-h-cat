using UnityEngine;
using UnityEngine.InputSystem;

public class QuestRewardTrigger : MonoBehaviour {
    [Header("Reward Settings")]
    public int foodReward = 5; 
    
    private GameObject currentStaticPlayer = null;
    private bool questCompleted = false; 

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !questCompleted) {
            currentStaticPlayer = collision.gameObject;
            Debug.Log("E für Food");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            currentStaticPlayer = null;
        }
    }

    private void Update() {
        if (currentStaticPlayer != null && !questCompleted && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) {
            ClaimReward();
        }
    }

    private void ClaimReward() {
        FoodManager foodManager = FindAnyObjectByType<FoodManager>();
        if (foodManager != null) {
            foodManager.AddFood(foodReward);
            Debug.Log("Belohnung erhalten!!!!!!!!!!!!!!!!!!!!!");
        }
    }
}