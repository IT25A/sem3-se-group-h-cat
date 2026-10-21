using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private int collectedCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prüfen, ob das berührte Objekt das Tag "Collectible" hat
        if (collision.CompareTag("Collectible"))
        {
            collectedCount++;
            Debug.Log("Gegenstand eingesammelt! Gesamt: " + collectedCount);

            // Zerstört das Objekt, das berührt wurde 
            Destroy(collision.gameObject);
        }
    }
}