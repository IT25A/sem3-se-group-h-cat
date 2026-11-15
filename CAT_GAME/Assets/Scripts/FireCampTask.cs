using UnityEngine;

public class FireCampTask : MonoBehaviour
{
    [Header("Reward Configuration")]
    public int reward = 10;
    public int requiredWood = 10;

    [Header("Visual Elements")]
    public GameObject campfireVisual;

    // Global state checked exclusively by CollectibleItem.cs for Wood type
    public static bool isTaskActive = false;

    private bool rewardGiven = false;
    private bool wasInDialogue = false;
    private NPCChoiceTalk npcTalk;

    private void Awake()
    {
        isTaskActive = false;
    }

    private void Start()
    {
        npcTalk = GetComponent<NPCChoiceTalk>();
        if (npcTalk == null)
        {
            Debug.LogError("[FireCampTask] ERROR: No NPCChoiceTalk found on this object!");
        }

        if (campfireVisual != null)
        {
            campfireVisual.SetActive(false);
        }
    }

    // Called automatically or externally when activating the quest
    public static void ActivateFireQuest()
    {
        isTaskActive = true;
        Debug.Log("[FireCampTask] Campfire quest activated!");
    }

    private void Update()
    {
        if (npcTalk == null || rewardGiven) return;

        // 1. External Detection: Automatically activate wood picking when reaching the wood choice line
        if (!isTaskActive && npcTalk.IsTalking())
        {
            if (npcTalk.dialogueData == npcTalk.AfterMouse && npcTalk.GetCurrentIndex() == npcTalk.fireQuestStartIndex)
            {
                ActivateFireQuest();
            }
        }

        // 2. Completion Check: Detect when dialogue finishes after collecting all wood
        if (isTaskActive)
        {
            if (npcTalk.IsTalking())
            {
                if (npcTalk.dialogueData == npcTalk.AfterMouse)
                {
                    wasInDialogue = true;
                }
            }
            else if (wasInDialogue)
            {
                wasInDialogue = false;

                if (TaskCollectibleState.WoodCount >= requiredWood)
                {
                    rewardGiven = true;

                    if (campfireVisual != null)
                    {
                        campfireVisual.SetActive(true);
                    }

                    if (FoodManager.Instance != null)
                    {
                        FoodManager.Instance.AddFood(reward);
                        Debug.Log($"[FireCampTask] Campfire lit! Added {reward} food successfully!");
                    }
                    else
                    {
                        Debug.LogError("[FireCampTask] FoodManager.Instance is NULL!");
                    }

                    TaskCollectibleState.ResetTaskCollectibles();
                }
                else
                {
                    Debug.Log($"[FireCampTask] Not enough wood: {TaskCollectibleState.WoodCount}/{requiredWood}");
                }
            }
        }
    }
}