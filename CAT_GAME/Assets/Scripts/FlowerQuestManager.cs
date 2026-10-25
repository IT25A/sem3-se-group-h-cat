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

    // List used to store which flowers have already been collected (using their unique names or IDs)
    private HashSet<string> collectedFlowerIDs = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // Prevent the manager from being destroyed when changing scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another manager already exists in the new scene, destroy the duplicate
            Destroy(gameObject);
        }
    }

    public void CollectFlower(string flowerID)
    {
        if (isQuestCompleted) return;

        // Prevent the same flower from being counted twice in case of a bug
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

    // Method used by the flower to check if it has already been collected before the player sees it
    public bool IsFlowerCollected(string flowerID)
    {
        return collectedFlowerIDs.Contains(flowerID);
    }

    private void CompleteQuest()
    {
        isQuestCompleted = true;

        Debug.Log("Quest Completed: All specific flowers collected!");
        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.ShowLocalizedNotification(tableName, warningKey);
            Debug.Log("[Quest] 10 Blumen erreicht - Notification gesendet!");
        }
    }
}
