using UnityEngine;

// Keeps the camera instance persistent across scene transitions and prevents duplicates (Singleton)
public class PersistentCameraManager : MonoBehaviour
{
    private static PersistentCameraManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}