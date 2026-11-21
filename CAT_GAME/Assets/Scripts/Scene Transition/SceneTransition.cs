using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Scene Transition Configuration")]
    public string targetSceneName;

    [InspectorName("Spawn ID in New Scene")]
    [Tooltip("Target SpawnPoint ID in the new scene. If empty, the last return point will be used.")]
    public string targetSpawnID;

    [InspectorName("ID for returning")]
    [Tooltip("Unique ID of this transition so the target scene knows the return point.")]
    public string returnID;

    [Header("End Scene Settings")]
    [Tooltip("Check this if this transition leads to the final scene and should destroy the player.")]
    public bool destroyPlayerOnTransition = false;

    // Controlled by door lock scripts (e.g., QuestDoor, NPCDoorLock) to block scene transitions
    public bool isLocked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isLocked)
            {
                return;
            }

            if (!string.IsNullOrEmpty(returnID))
            {
                SceneData.LastSpawnID = returnID;
            }

            // If no fixed target ID is specified, return to the last saved spawn point
            if (!string.IsNullOrEmpty(targetSpawnID))
            {
                SceneData.TargetSpawnID = targetSpawnID;
            }
            else
            {
                SceneData.TargetSpawnID = SceneData.LastSpawnID;
            }

            // Destroy the player object before loading the new scene if enabled
            if (destroyPlayerOnTransition)
            {
                Destroy(collision.gameObject);
            }

            isLocked = true;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}