using UnityEngine;

// Hält das Spieler-Objekt über Szenenwechsel hinweg persistent und verhindert Duplikate (Singleton)
public class PersistentPlayer : MonoBehaviour
{
    private static PersistentPlayer instance;

    private void Awake()
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