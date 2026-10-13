using System.Collections;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    private void Start()
    {
        // Iniciamos una corrutina para asegurar que la escena y los objetos estén listos
        StartCoroutine(MovePlayerToSpawnPointRoutine());
    }

    private IEnumerator MovePlayerToSpawnPointRoutine()
    {
        // Esperamos un pequeño instante (un frame) para que todo se estabilice al cambiar de escena
        yield return null;

        // Busca al jugador en la escena mediante su Tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && !string.IsNullOrEmpty(SceneData.TargetSpawnID))
        {
            // Busca todos los componentes SpawnPoint activos en la nueva escena
            SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

            foreach (SpawnPoint sp in spawnPoints)
            {
                // Si encontramos el SpawnPoint con el mismo ID guardado, movemos al jugador ahí
                if (sp.spawnID == SceneData.TargetSpawnID)
                {
                    player.transform.position = sp.transform.position;
                    break;
                }
            }
        }
    }
}