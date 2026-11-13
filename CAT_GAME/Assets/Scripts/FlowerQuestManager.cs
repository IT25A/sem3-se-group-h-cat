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

    // Speichert die eindeutigen IDs bereits gesammelter Blumen über Szenenwechsel hinweg
    private HashSet<string> collectedFlowerIDs = new HashSet<string>();

    private void Awake()
    {
        // Singleton-Muster: Instanz szenenübergreifend behalten und Duplikate zerstören
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

        // Verhindert, dass dieselbe Blume mehrfach gezählt wird
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
