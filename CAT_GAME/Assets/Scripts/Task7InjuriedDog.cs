using UnityEngine;

public class Task7InjuriedDog : MonoBehaviour
{
    [Header("Reward")]
    public int reward = 15;
    private bool rewardGiven = false;
    private NPCTalk npcTalk;

    void Start()
    {
        npcTalk = GetComponent<NPCTalk>();
        if (npcTalk == null)
        {
            Debug.LogError("[Task7InjuriedDog] FEHLER: Kein NPCTalk auf diesem Objekt gefunden!");
        }
    }

    void Update()
    {
        if (npcTalk == null || rewardGiven) return;

        // Belohnung vergeben, sobald der verletzte Hund beim Arzt abgegeben und das Gespräch beendet wurde
        if (npcTalk.requiredFollower.hasStayedAtDoctor && !npcTalk.IsTalking())
        {
            GiveRewardAndNotification();
        }
    }

    private void GiveRewardAndNotification()
    {
        rewardGiven = true;

        if (FoodManager.Instance != null)
        {
            FoodManager.Instance.AddFood(reward);
            Debug.Log($"[Task7InjuriedDog] {reward} Food erfolgreich nach dem Beenden des Dialogs hinzugefügt!");
        }
        else
        {
            Debug.LogError("[Task7InjuriedDog] FoodManager.Instance ist NULL!");
        }
    }
}