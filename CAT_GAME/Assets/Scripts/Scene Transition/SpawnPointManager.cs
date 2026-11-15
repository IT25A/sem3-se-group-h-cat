using System.Collections;
using UnityEngine;

/// Handles relocating the persistent player to the correct spawn point upon scene load.
public class SpawnPointManager : MonoBehaviour
{
    private void Start()
    {
        // Start a routine to delay positioning by one frame for scene stability
        StartCoroutine(MovePlayerToSpawnPointRoutine());
    }
    private IEnumerator MovePlayerToSpawnPointRoutine()
    {
        // Wait for one frame to ensure all scene components are fully initialized
        yield return null;

        // Find the player in the newly loaded scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && !string.IsNullOrEmpty(SceneData.TargetSpawnID))
        {
            // Find all active SpawnPoint components in the scene
            SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsInactive.Exclude);

            foreach (SpawnPoint sp in spawnPoints)
            {
                // Teleport player if the spawn point ID matches the target
                if (sp.spawnID == SceneData.TargetSpawnID)
                {
                    player.transform.position = sp.transform.position;
                    break;
                }
            }
        }
    }
}
