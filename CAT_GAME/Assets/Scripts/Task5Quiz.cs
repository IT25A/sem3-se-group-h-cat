using UnityEngine;

public class Task5Quiz : MonoBehaviour
{
    [Header("Reward")]
    public int reward = 20;
    private bool rewardGiven = false;
    private bool wasInCorrectDialogue = false;
    private NPCChoiceTalk npcTalk;

    [Header("Quiz State")]
    public bool playerWonQuiz = false;
    void Start()
    {
        npcTalk = GetComponent<NPCChoiceTalk>();
        if (npcTalk == null)
        {
            Debug.LogError("[Task5Quiz] FEHLER: Kein NPCChoiceTalk auf diesem Objekt gefunden!");
        }
    }

    void Update()
    {
        if (npcTalk == null || rewardGiven) return;

        if (playerWonQuiz && !npcTalk.IsTalking())
        {
            GiveRewardAndNotification();
        }
    }

    private void GiveRewardAndNotification()
    {
        rewardGiven = true;
        playerWonQuiz = false;

        if (FoodManager.Instance != null)
        {
            FoodManager.Instance.AddFood(reward);
            Debug.Log($"[Task5Quiz] {reward} Food erfolgreich nach dem Beenden des Dialogs hinzugefügt!");
            SceneData.isQuizFireWoodKeyCompleted = true;
        }
        else
        {
            Debug.LogError("[Task5Quiz] FoodManager.Instance ist NULL!");
        }
    }
}