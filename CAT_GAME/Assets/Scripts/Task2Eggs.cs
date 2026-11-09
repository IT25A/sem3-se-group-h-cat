using UnityEngine;

public class Task2Eggs : MonoBehaviour
{
    public int requiredAmount = 10;
    public int rewardAmount = 10;

    void Awake()
    {
        int eggCount = TaskCollectibleState.EggCount;
        if (eggCount >= requiredAmount)
        {
            FoodManager.Instance.AddFood(rewardAmount);
            TaskCollectibleState.ResetTaskCollectibles();
        }
    }

    void Update()
    {
        
    }
}
