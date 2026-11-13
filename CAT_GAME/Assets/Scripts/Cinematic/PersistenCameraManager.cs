using UnityEngine;

// Hält die Kamera-Instanz über Szenenwechsel hinweg persistent und verhindert Duplikate (Singleton)
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