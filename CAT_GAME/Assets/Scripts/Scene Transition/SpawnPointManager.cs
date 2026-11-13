using System.Collections;
using UnityEngine;

// Versetzt den persistenten Spieler nach dem Laden einer Szene an den passenden SpawnPoint
public class SpawnPointManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(MovePlayerToSpawnPointRoutine());
    }
    private IEnumerator MovePlayerToSpawnPointRoutine()
    {
        // Einen Frame warten, damit alle Szenen-Komponenten vollständig initialisiert sind
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
