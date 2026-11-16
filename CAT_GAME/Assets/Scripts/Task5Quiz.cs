using UnityEngine;

public class Task5Quiz : MonoBehaviour
{
    [Header("Reward")]
    public int reward = 20;
    private bool rewardGiven = false;
    private bool wasInCorrectDialogue = false;
    private NPCChoiceTalk npcTalk;

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

        if (npcTalk.IsTalking())
        {
            if (npcTalk.dialogueData == npcTalk.AfterMouse && npcTalk.GetCurrentIndex() == npcTalk.quizCorrectAnswerIndex)
            {
                wasInCorrectDialogue = true;
            }
        }
        else if (wasInCorrectDialogue)
        {
            Debug.Log("[Task5Quiz] Quiz-Hint-Dialog beendet. Belohnung wird vergeben!");

            rewardGiven = true;
            wasInCorrectDialogue = false;

            if (FoodManager.Instance != null)
            {
                FoodManager.Instance.AddFood(reward);
                Debug.Log($"[Task5Quiz] {reward} Food erfolgreich nach dem Quiz hinzugefügt!");
                SceneData.isQuizFireWoodKeyCompleted = true;
            }
            else
            {
                Debug.LogError("[Task5Quiz] FoodManager.Instance ist NULL!");
            }
        }
    }
}
