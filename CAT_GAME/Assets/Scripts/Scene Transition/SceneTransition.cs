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

    // Controlled by door lock scripts (e.g., QuestDoor, NPCDoorLock) to block scene transitions
    [HideInInspector] public bool isLocked = false;
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

            isLocked = true; 
            SceneManager.LoadScene(targetSceneName);
        }
    }
}