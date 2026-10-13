using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Configuración de Escena")]
    public string targetSceneName;
    public string targetSpawnID;

    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Solo se activa si el jugador ya no está "bloqueado" por haber aparecido aquí
            if (!playerInside)
            {
                SceneData.TargetSpawnID = targetSpawnID;
                SceneManager.LoadScene(targetSceneName);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // En cuanto el jugador se aleja de la puerta, permitimos que vuelva a funcionar
            playerInside = false;
        }
    }

    // Opcional: Si el jugador aparece justo encima, marcamos que está dentro para que no se active de inmediato
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !playerInside)
        {
            // Si acaba de spawnear aquí, bloqueamos la salida hasta que se mueva y salga
            // (Esto se puede complementar asegurando que el SpawnPoint esté fuera del trigger)
        }
    }
}