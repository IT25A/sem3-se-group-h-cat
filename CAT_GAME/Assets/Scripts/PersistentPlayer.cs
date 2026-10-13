using UnityEngine;

/// Ensures the GameObject persists across scene loads without being destroyed.
/// Prevents duplicates if another instance already exists in the new scene.
public class PersistentPlayer : MonoBehaviour
{
    private static PersistentPlayer instance;

    private void Awake()
    {
        // Enforce Singleton pattern: if an instance already exists, destroy this duplicate
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set this instance as the unique global instance
        instance = this;

        // Keep this GameObject alive across scene transitions
        DontDestroyOnLoad(gameObject);
    }
}