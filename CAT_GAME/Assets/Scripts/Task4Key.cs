using UnityEngine;

public class Task4Key : MonoBehaviour
{
    [Header("Reward")]
    public int reward = 10;
    public int requiredAmount = 1;
    private bool rewardGiven = false;
    private bool wasInDialogue = false;
    private NPCChoiceTalk npcTalk;

    void Start()
    {
        npcTalk = GetComponent<NPCChoiceTalk>();
        if (npcTalk == null)
        {
            Debug.LogError("[Task4Key] FEHLER: Kein NPCChoiceTalk auf diesem Objekt gefunden!");
        }
    }

    void Update()
    {
        if (npcTalk == null || rewardGiven) return;

        // Track when the completion dialogue (AfterKey) is active and only grant the reward after the dialogue closes
        if (npcTalk.IsTalking())
        {
            if (npcTalk.dialogueData == npcTalk.AfterKey)
            {
                wasInDialogue = true;
            }
        }
        else if (wasInDialogue)
        {
            Debug.Log("[Task4Key] DIALOG WURDE GESCHLOSSEN - Belohnung wird geprüft!");

            rewardGiven = true;
            wasInDialogue = false;

            if (TaskCollectibleState.KeyCount >= requiredAmount)
            {
                if (FoodManager.Instance != null)
                {
                    FoodManager.Instance.AddFood(reward);
                    Debug.Log($"[Task4Key] {reward} Food erfolgreich hinzugefügt!");
                }
                else
                {
                    Debug.LogError("[Task4Key] FoodManager.Instance ist NULL!");
                }
            }
            SceneData.savedKeyCount = TaskCollectibleState.KeyCount;
            SceneData.isQuizFireWoodKeyCompleted = true;
            TaskCollectibleState.ResetTaskCollectibles();
        }
    }
}