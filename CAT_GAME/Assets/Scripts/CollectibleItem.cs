using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectibleItem : MonoBehaviour
{
    [Tooltip("Leave empty to auto-generate from scene name and position")]
    public string uniqueId;

    private void Awake()
    {
        if (string.IsNullOrEmpty(uniqueId))
        {
            // Auto-generates an ID based on scene and position
            uniqueId = $"{SceneManager.GetActiveScene().name}_{transform.position.x:F2}_{transform.position.y:F2}";
        }

        // If already collected during this task, destroy immediately on scene load
        if (TaskCollectibleState.IsCollected(uniqueId))
        {
            Destroy(gameObject);
        }
    }
}