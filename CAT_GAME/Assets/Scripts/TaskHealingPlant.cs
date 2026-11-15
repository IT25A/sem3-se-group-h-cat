
using UnityEngine;
using UnityEngine.Events;

public class HealingPlantTask : MonoBehaviour
{
    [Header("Task Settings")]
    [SerializeField] private int requiredPlants = 1;

    [Header("Task Completion")]
    [SerializeField] private UnityEvent onTaskCompleted;

    private bool taskCompleted = false;

    // Checks if the player has collected the required number of plants
    public void CheckTaskCompletion()
    {
        if (taskCompleted)
        {
            return;
        }

        if (TaskCollectibleState.HealingPlant >= requiredPlants)
        {
            CompleteTask();
        }
        else
        {
            Debug.Log("The HealingPlant is still missing.");
        }
    }

    // Completes the task
    private void CompleteTask()
    {
        if (taskCompleted)
        {
            return;
        }

        taskCompleted = true;

        Debug.Log("HealingPlant task completed!");

        // Invokes the events assigned in the Inspector
        onTaskCompleted?.Invoke();
    }

    // Returns whether the task has already been completed
    public bool IsTaskCompleted()
    {
        return taskCompleted;
    }
}
