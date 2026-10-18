using UnityEngine;

public class QuestFlower : MonoBehaviour
{
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the Player
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Notify the manager
            if (FlowerQuestManager.Instance != null)
            {
                FlowerQuestManager.Instance.CollectFlower();
            }

            // Remove the flower from the game
            Destroy(gameObject);
        }
    }
}