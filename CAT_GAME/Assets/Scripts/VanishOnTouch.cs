using UnityEngine;

public class VanishOnTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prüft, ob das Objekt, das in den Trigger tritt, der Spieler ist
        if (collision.CompareTag("Player"))
        {
            // Zerstört das GameObject, an dem dieses Skript hängt (den NPC)
            Destroy(gameObject);
        }
    }
}