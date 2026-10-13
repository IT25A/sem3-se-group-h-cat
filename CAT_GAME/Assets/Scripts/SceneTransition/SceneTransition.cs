using UnityEngine;
using UnityEngine.SceneManagement;

/// Triggers a scene change when the player enters the trigger area.
public class SceneTransition : MonoBehaviour
{
    [Header("Target Scene Configuration")]
    public string targetSceneName;
    public string targetSpawnID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the Player
        if (collision.CompareTag("Player"))
        {
            // Store the destination spawn ID globally before loading
            SceneData.TargetSpawnID = targetSpawnID;

            // Load the specified scene
            SceneManager.LoadScene(targetSceneName);
        }
    }
}