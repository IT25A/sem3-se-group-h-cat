using UnityEngine;

// Marks a named spawn point in the scene and places the player there on load if the ID matches
public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Unique identifier for this spawn point (e.g., 'House_Entrance', 'Forest_Exit')")]
    public string spawnID;

    private void Start()
    {
        if (!string.IsNullOrEmpty(SceneData.TargetSpawnID) && SceneData.TargetSpawnID == spawnID)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = transform.position;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}