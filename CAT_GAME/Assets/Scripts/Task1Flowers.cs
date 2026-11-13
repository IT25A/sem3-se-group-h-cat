using UnityEngine;

public class QuestFlower : MonoBehaviour
{
    public string flowerID;
    private bool isCollected = false;

    private void Awake()
    {
        // Erzeugt eine eindeutige ID anhand von Szenenname und Weltposition
        flowerID = gameObject.scene.name + "_" + transform.position.x + "_" + transform.position.y;
    }

    private void Start()
    {
        // Zerstört die Blume beim Szenenstart sofort, falls sie bereits gesammelt wurde
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