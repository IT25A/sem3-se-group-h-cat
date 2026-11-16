using UnityEngine;

// Checks on start whether enough eggs have been collected, grants the food reward, and resets the collection state
public class Task2Eggs : MonoBehaviour
{
    public int requiredAmount = 10;
    public int rewardAmount = 10;

    void Start()
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
