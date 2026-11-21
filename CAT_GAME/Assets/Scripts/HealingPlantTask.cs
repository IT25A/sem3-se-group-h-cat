using UnityEngine;
using UnityEngine.Events;

public class HealingPlantTask : MonoBehaviour
{
    [Header("Task Settings")]
    [SerializeField] private int requiredPlants = 1;

    [Header("Task Completion")]
    [SerializeField] private UnityEvent onTaskCompleted;

    [Header("Reward")]
    public int reward = 20;
    private bool rewardGiven = false;
    private bool wasInDialogue = false;
    private NPCTalk npcTalk;
    private bool taskCompleted = false;
    public SceneTransition finalDoor;

    // Call this method when talking to the doctor to check if the player has the plant
    public void CheckHealingPlant()
    {
        if (taskCompleted) return;

        // Check if the player has collected enough healing plants
        if (TaskCollectibleState.HealingPlantCount >= requiredPlants)
        {
            taskCompleted = true;
            Debug.Log("Doctor: Thank you for the healing plant!");

            // Trigger events assigned in the Inspector (like changing dialogue or opening a door)
            onTaskCompleted?.Invoke();
        }
        else
        {
            Debug.Log("Doctor: You still don't have the healing plant.");
        }
    }

    void Start()
    {
        npcTalk = GetComponent<NPCTalk>();
        if (npcTalk == null)
        {
            Debug.LogError("FEHLER: Kein NPCTalk auf diesem Objekt gefunden!");
        }
    }

    void Update()
    {
        if (npcTalk == null || rewardGiven) return;

        // Wait until the completion dialogue after returning the collar has finished before granting the reward
        if (npcTalk.IsPlayingAfterDoctorDialog())
        {
            wasInDialogue = true;
        }
        else if (!npcTalk.IsTalking() && wasInDialogue)
        {
            Debug.Log("[TaskHealingPlant] Finaler Arzt-Dialog geschlossen - Belohnung wird vergeben!");
            GiveRewardAndNotification();
        }
    }

    private void GiveRewardAndNotification()
    {
        rewardGiven = true;
        reward = 100 - SceneData.savedFood;

        if (FoodManager.Instance != null)
        {
            FoodManager.Instance.AddFood(reward);
            Debug.Log($"[Task8collar] {reward} Food erfolgreich nach dem Beenden des Dialogs hinzugefügt!");
        }
        else
        {
            Debug.LogError("[TaskHealingPlant] FoodManager.Instance ist NULL!");
        }

        // Unlock the final door so the player can pass to the end scene
        if (finalDoor != null)
        {
            finalDoor.isLocked = false;
            Debug.Log("[TaskHealingPlant] SUCCESS: Final door is now UNLOCKED!");
        }
        else
        {
            Debug.LogWarning("[TaskHealingPlant] Warning: Final Door reference is missing in the Inspector!");
        }

        // Lock the healing plant notification and reset the collection state after receiving the reward
        TaskCollectibleState.HealingPlantNotificationSent = true;
        TaskCollectibleState.ResetTaskCollectibles();
    }
}

