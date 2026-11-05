using UnityEngine;

public class Task4Key : MonoBehaviour
{
    [Header("Reward")]
    public int reward = 10;

    public int requiredAmount = 1;
    private bool rewardGiven = false;
    private NPCChoiceTalk npcTalk;
    void Start()
    {
        npcTalk = GetComponent<NPCChoiceTalk>();
        TaskCollectibleState.ResetTaskCollectibles();
    }

    void Update()
    {
        if (npcTalk == null || rewardGiven) return;


        if (TaskCollectibleState.KeyCount >= requiredAmount && !rewardGiven)
        {
            if (FoodManager.Instance != null)
            {
                FoodManager.Instance.AddFood(reward);
                Debug.Log($"[Quest] {reward} Food nach Dialogende erfolgreich hinzugefügt!");
            }
        }

        TaskCollectibleState.ResetTaskCollectibles();
    }
}
