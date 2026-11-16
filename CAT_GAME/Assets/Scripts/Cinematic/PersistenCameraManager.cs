using UnityEngine;

public class PersistentCameraManager : MonoBehaviour
{
    private static PersistentCameraManager instance;

    void Awake()
    {
        // Prevent duplicate camera instances when returning to the initial scene
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        // Keep this GameObject (along with its Camera and Cinemachine setup) alive across scene transitions
        DontDestroyOnLoad(gameObject);
    }
}