using UnityEngine;

public class FlowerQuestManager : MonoBehaviour
{
    public static FlowerQuestManager Instance { get; private set; }

    [Header("Quest Settings")]
    [SerializeField] private int requiredFlowers = 10;

    [Header("Quest Progress")]
    public int currentFlowerCount = 0;
    public bool isQuestCompleted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectFlower()
    {
        if (isQuestCompleted) return;

        currentFlowerCount++;
        Debug.Log($"Flower collected: {currentFlowerCount}/{requiredFlowers}");

        if (currentFlowerCount >= requiredFlowers)
        {
            CompleteQuest();
        }
    }

    private void CompleteQuest()
    {
        isQuestCompleted = true;
        Debug.Log("Quest Completed: All 10 flowers collected!");

        // Open doors, give rewards, show UI, etc.
    }
}