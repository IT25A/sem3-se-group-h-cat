using UnityEngine;

public class Task8collar : MonoBehaviour
{
    [Header("Reward")]
    public int reward = 10;
    private bool rewardGiven = false;
    private NPCTalk npcTalk;

    void Start()
    {
        npcTalk = GetComponent<NPCTalk>();
        if (npcTalk == null)
        {
            Debug.LogError("[Task8collar] FEHLER: Kein NPCTalk auf diesem Objekt gefunden!");
        }
    }

    void Update()
    {
        if (npcTalk == null || rewardGiven) return;

        if (npcTalk.afterItemDialogue != null && !npcTalk.IsTalking() && TaskCollectibleState.SimbaNecklaceCount >= 1)
        {
            GiveRewardAndNotification();
            TaskCollectibleState.simbaNecklaceNotificationSent = true; // Set the notification flag to true after giving the reward
        }
    }

    private void GiveRewardAndNotification()
    {
        rewardGiven = true;

        if (FoodManager.Instance != null)
        {
            FoodManager.Instance.AddFood(reward);
            Debug.Log($"[Task8collar] {reward} Food erfolgreich nach dem Beenden des Dialogs hinzugefügt!");
        }
        else
        {
            Debug.LogError("[Task8collar] FoodManager.Instance ist NULL!");
        }

        TaskCollectibleState.ResetTaskCollectibles(); // Reset the collectible state after giving the reward
    }
}