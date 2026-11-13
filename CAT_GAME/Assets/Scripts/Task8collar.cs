using UnityEngine;

public class Task8collar : MonoBehaviour
{
    [Header("Reward")]
    public int reward = 10;
    private bool rewardGiven = false;
    private bool wasInDialogue = false;
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

        // Warten, bis der Abschlussdialog nach Abgabe des Halsbands beendet wurde, bevor die Belohnung vergeben wird
        if (npcTalk.IsPlayingAfterItemDialogue())
        {
            wasInDialogue = true;
        }
        else if (!npcTalk.IsTalking() && wasInDialogue)
        {
            Debug.Log("[Task8collar] Finaler Arzt-Dialog geschlossen - Belohnung wird vergeben!");
            GiveRewardAndNotification();
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

        // Nach Erhalt der Belohnung die Halsband-Benachrichtigung sperren und den Sammelstatus zurücksetzen
        TaskCollectibleState.simbaNecklaceNotificationSent = true;
        TaskCollectibleState.ResetTaskCollectibles();
    }
}