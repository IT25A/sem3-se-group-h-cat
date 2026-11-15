using UnityEngine;
using System.Collections.Generic;

public class FlowerQuestManager : MonoBehaviour
{
    public static FlowerQuestManager Instance { get; private set; }

    [Header("Quest Settings")]
    [SerializeField] private int requiredFlowers = 10;

    [Header("Quest Progress")]
    public int currentFlowerCount = 0;
    public bool isQuestCompleted = false;
    public bool finalDialogueFinished = false;

    [Header("Localization Keys")]
    public string tableName = "MenuText";
    public string warningKey = "received_flowers";

    // Stores the unique IDs of already collected flowers across scene transitions
    private HashSet<string> collectedFlowerIDs = new HashSet<string>();

    private void Awake()
    {
        // Singleton pattern: persist instance across scenes and destroy duplicates
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectFlower(string flowerID)
    {
        if (isQuestCompleted) return;

        // Prevents the same flower from being counted multiple times
        if (!collectedFlowerIDs.Contains(flowerID))
        {
            collectedFlowerIDs.Add(flowerID);
            currentFlowerCount++;

            Debug.Log($"Flower collected: {currentFlowerCount}/{requiredFlowers}");

            if (currentFlowerCount >= requiredFlowers)
            {
                CompleteQuest();
            }
        }
    }

    public bool IsFlowerCollected(string flowerID)
    {
        return collectedFlowerIDs.Contains(flowerID);
    }

    private void CompleteQuest()
    {
        isQuestCompleted = true;

        Debug.Log("Quest Completed: All specific flowers collected!");
    }
}
