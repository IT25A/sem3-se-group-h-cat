using UnityEngine;
using UnityEngine.SceneManagement;

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

    // We make it private so normal doors never touch it, 
    // but QuestDoor can still modify it safely.
    [HideInInspector] public bool isLocked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // If the door is locked by a quest, stop execution
            if (isLocked)
            {
                return;
            }

            if (!string.IsNullOrEmpty(returnID))
            {
                SceneData.LastSpawnID = returnID;
            }

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