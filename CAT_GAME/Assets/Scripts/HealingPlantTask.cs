using UnityEngine;
using UnityEngine.Events;

public class HealingPlantTask : MonoBehaviour
{
    [Header("Task Settings")]
    [SerializeField] private int requiredPlants = 1;

    [Header("Task Completion")]
    [SerializeField] private UnityEvent onTaskCompleted;

    private bool taskCompleted = false;

    // Call this method when talking to the doctor to check if the player has the plant
    public void CheckHealingPlant()
    {
        if (taskCompleted) return;

        // Check if the player has collected enough healing plants
        if (TaskCollectibleState.HealingPlantCount >= requiredPlants)
        {
            taskCompleted = true;
            Debug.Log("Doctor: Thank you for the healing plant!");

            // Trigger events assigned in the Inspector (like changing dialogue or opening a door)
            onTaskCompleted?.Invoke();
        }
        else
        {
            Debug.Log("Doctor: You still don't have the healing plant.");
        }
    }
}