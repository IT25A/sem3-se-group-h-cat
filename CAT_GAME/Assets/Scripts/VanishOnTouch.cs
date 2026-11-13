using UnityEngine;

public class VanishOnTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Zerstört dieses Objekt, sobald der Spieler den Trigger berührt
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}