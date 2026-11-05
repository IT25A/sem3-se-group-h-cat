using UnityEngine;
using System.Collections;

public class Task3Mouse : MonoBehaviour
{
    public int requiredAmount = 1;
    public int rewardAmount = 15;
    private bool rewardGiven = false;
    void Start()
    {
    }
    void Update()
    {
        if (!rewardGiven && TaskCollectibleState.MouseCount >= requiredAmount && TaskCollectibleState.mouseNotificationSent)
        {
            StartCoroutine(WaitAndGiveFoodAfterMouseNotification());
        }
    }
    IEnumerator WaitAndGiveFoodAfterMouseNotification()
    {
        rewardGiven = true;

        yield return new WaitForSeconds(3.0f);

        if (FoodManager.Instance != null)
        {
            FoodManager.Instance.AddFood(rewardAmount);
        }
        SceneData.savedMouseCount = TaskCollectibleState.MouseCount;
        TaskCollectibleState.ResetTaskCollectibles();
    }
}
