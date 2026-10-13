using UnityEngine;
using UnityEngine.SceneManagement;

/// Triggers a scene change when the player enters the trigger area.
/// Supports returning to the last visited spawn point automatically.

public class SceneTransition : MonoBehaviour
{
    [Header("Scene Transition Configuration")]
    public string targetSceneName;

    [InspectorName("Spawn ID in New Scene")]
    [Tooltip("Target SpawnPoint ID. If left empty, it will default to the player's last spawn point.")]
    public string targetSpawnID;

    [InspectorName("ID for returning")]
    [Tooltip("Unique ID for THIS door/transition so the target scene knows where to return.")]
    public string returnID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Store this door's ID as the last origin point
            if (!string.IsNullOrEmpty(returnID))
            {
                SceneData.LastSpawnID = returnID;
            }

            // Use specified target spawn ID, or fallback to last spawn ID
            if (!string.IsNullOrEmpty(targetSpawnID))
            {
                SceneData.TargetSpawnID = targetSpawnID;
            }
            else
            {
                SceneData.TargetSpawnID = SceneData.LastSpawnID;
            }

            SceneManager.LoadScene(targetSceneName);
        }
    }
}
