using System.Collections;
using UnityEngine;

public class FlowerQuestCheck : MonoBehaviour
{

    public NPCDialogue startDialog;     // Initial dialogue: task to collect 10 flowers
    public NPCDialogue inbetweenDialog; // Intermediate dialogue: not enough flowers collected yet
    public NPCDialogue finalDialog;     // Final dialogue: all 10 flowers have been collected

    [Header("Reward")]
    public int reward = 10;
    private NPCTalk npcTalk;
    private bool dialogueActive = false;

    void Start()
    {
        npcTalk = GetComponent<NPCTalk>();
        npcTalk.dialogueData = startDialog;
    }

    void Update()
    {
        // Detects when the final dialogue finishes in order to grant the reward once
        if (dialogueActive && !npcTalk.isDialogueActive)
        {
            if (npcTalk.dialogueData == finalDialog && SceneData.flowerQuestCompleted == false)
            {
                if (FlowerQuestManager.Instance != null)
                {
                    FlowerQuestManager.Instance.finalDialogueFinished = true;
                }

                if (FoodManager.Instance != null)
                {
                    FoodManager.Instance.AddFood(reward);
                    Debug.Log($"[Quest] {reward} Food hinzugefügt!");
                    SceneData.flowerQuestCompleted = true;
                }
            }
        }

        dialogueActive = npcTalk.isDialogueActive;
        if (FlowerQuestManager.Instance != null)
        {
            int current = FlowerQuestManager.Instance.currentFlowerCount;
            bool finished = FlowerQuestManager.Instance.finalDialogueFinished;

        }

        // Only update dialogue data when the dialogue window is closed to prevent switching text mid-conversation
        if (npcTalk.isDialogueActive)
        {
            return;
        }
        if (FlowerQuestManager.Instance != null)
        {
            if (FlowerQuestManager.Instance.currentFlowerCount >= 10)
            {
                npcTalk.dialogueData = finalDialog;
            }
            else
            {
                npcTalk.dialogueData = inbetweenDialog;
            }
        }
    }
}