using UnityEngine;

public class FireCampTask : MonoBehaviour
{
    [Header("Reward Configuration")]
    public int reward = 10;
    public int requiredWood = 10;

    [Header("Visual Elements")]
    public GameObject campfireVisual;

    public static bool isTaskActive = false;
    private bool rewardGiven = false;
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

    public static void ActivateFireQuest()
    {
        isTaskActive = true;
        Debug.Log("[FireCampTask] Campfire quest activated!");
    }

    private void Update()
    {
        if (npcTalk == null || rewardGiven) return;

        // Quest aktivieren, sobald der Spieler im Dialog die Feuerholz-Option wählt
        if (!isTaskActive && npcTalk.IsTalking())
        {
            if (npcTalk.dialogueData == npcTalk.AfterMouse && npcTalk.GetCurrentIndex() == npcTalk.fireQuestStartIndex)
            {
                ActivateFireQuest();
            }
        }

        // Wenn genug Holz gesammelt wurde und der Abschlussdialog läuft, Lagerfeuer entzünden und Belohnung vergeben
        if (isTaskActive && !rewardGiven)
        {
            if (npcTalk.dialogueData == npcTalk.AfterKey)
            {
                if (TaskCollectibleState.WoodCount >= requiredWood || SceneData.savedWoodCount >= requiredWood)
                {
                    GiveRewardAndLightFire();
                }
            }
        }
    }

    private void GiveRewardAndLightFire()
    {
        rewardGiven = true;

        if (campfireVisual != null)
        {
            campfireVisual.SetActive(true);
            Debug.Log("[FireCampTask] Campfire lit successfully!");
        }

        if (FoodManager.Instance != null)
        {
            FoodManager.Instance.AddFood(reward);
            Debug.Log($"[FireCampTask] Food added: {reward}");
        }
        else
        {
            Debug.LogError("[FireCampTask] FoodManager.Instance is NULL!");
        }

        SceneData.isQuizFireWoodKeyCompleted = true;
    }
}