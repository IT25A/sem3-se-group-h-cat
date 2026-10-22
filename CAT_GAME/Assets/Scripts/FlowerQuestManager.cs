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

    // Lista para guardar cuáles flores ya fueron recogidas (guardamos sus nombres o IDs únicos)
    private HashSet<string> collectedFlowerIDs = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Esto hace que el manager NO se destruya al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya existe otro manager en la nueva escena, destruimos el duplicado
            Destroy(gameObject);
        }
    }

    public void CollectFlower(string flowerID)
    {
        if (isQuestCompleted) return;

        // Evitamos contar la misma flor dos veces si por algo se buguea
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

    // Método para que la flor sepa si ya fue recogida antes de que el jugador la vea
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