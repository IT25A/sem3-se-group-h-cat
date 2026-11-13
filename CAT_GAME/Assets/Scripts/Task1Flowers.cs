using UnityEngine;

public class QuestFlower : MonoBehaviour
{
    public string flowerID;
    private bool isCollected = false;

    private void Awake()
    {
        // Generates a unique ID based on the scene name and world position
        flowerID = gameObject.scene.name + "_" + transform.position.x + "_" + transform.position.y;
    }

    private void Start()
    {
        // Immediately destroys the flower on scene start if it has already been collected
        if (FlowerQuestManager.Instance != null && FlowerQuestManager.Instance.IsFlowerCollected(flowerID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            if (FlowerQuestManager.Instance != null)
            {
                FlowerQuestManager.Instance.CollectFlower(flowerID);
            }

            Destroy(gameObject);
        }
    }
}