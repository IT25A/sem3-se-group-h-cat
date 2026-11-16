using System.Collections;
using UnityEngine;

// Moves the persistent player to the matching SpawnPoint after loading a scene
public class SpawnPointManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(MovePlayerToSpawnPointRoutine());
    }
    private IEnumerator MovePlayerToSpawnPointRoutine()
    {
        // Wait one frame so all scene components are fully initialized
        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && !string.IsNullOrEmpty(SceneData.TargetSpawnID))
        {
            SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsInactive.Exclude);

            foreach (SpawnPoint sp in spawnPoints)
            {
                if (sp.spawnID == SceneData.TargetSpawnID)
                {
                    player.transform.position = sp.transform.position;
                    break;
                }
            }
        }
    }
}
