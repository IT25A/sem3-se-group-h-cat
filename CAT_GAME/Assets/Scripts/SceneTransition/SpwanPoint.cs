using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Identificador único para este punto (ej. 'Entrada_Casa', 'Salida_Bosque')")]
    public string spawnID;

    private void OnDrawGizmos()
    {
        // Dibuja una esfera verde en la vista Scene de Unity para ver fácilmente dónde está el SpawnPoint
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}