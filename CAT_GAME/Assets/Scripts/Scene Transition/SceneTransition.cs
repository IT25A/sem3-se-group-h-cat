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

    // Wird von Tür-Sperrskripten (z. B. QuestDoor, NPCDoorLock) gesteuert, um den Szenenwechsel zu blockieren
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

            // Wenn keine feste Ziel-ID angegeben ist, zum zuletzt gespeicherten Spawn-Punkt zurückkehren
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