using UnityEngine;


/// Marker component to identify a spawn location within a scene.
public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Unique identifier for this spawn point (e.g., 'House_Entrance', 'Forest_Exit')")]
    public string spawnID;

    private void OnDrawGizmos()
    {
        // Draws a green wire sphere in Unity's Scene view to easily visualize the spawn location
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}