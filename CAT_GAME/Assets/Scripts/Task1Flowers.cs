using UnityEngine;

public class QuestFlower : MonoBehaviour
{
    public string flowerID;
    private bool isCollected = false;

    private void Awake()
    {
        // Create a unique ID for this flower based on its world position and the scene name
        // (Example: "SceneName_X12.5_Y5.1")
        flowerID = gameObject.scene.name + "_" + transform.position.x + "_" + transform.position.y;
    }

    private void Start()
    {
        // When the scene starts, check if this flower has already been collected
        if (FlowerQuestManager.Instance != null && FlowerQuestManager.Instance.IsFlowerCollected(flowerID))
        {
            // If it has already been collected, destroy it immediately so it does not appear
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Notify the manager and pass its unique ID
            if (FlowerQuestManager.Instance != null)
            {
                FlowerQuestManager.Instance.CollectFlower(flowerID);
            }

            // Destroy the current flower
            Destroy(gameObject);
        }
    }
}