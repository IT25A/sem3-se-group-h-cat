using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prüfen, ob das berührte Objekt das Tag "Collectible" hat
        if (collision.CompareTag("Egg"))
        {
            CollectibleItem item = collision.GetComponent<CollectibleItem>();
            string id = item != null ? item.uniqueId : null;

            TaskCollectibleState.RegisterCollectible(id);
            Debug.Log("Ei eingesammelt! Gesamt: " + TaskCollectibleState.CollectedCount);

            // Zerstört das Objekt, das berührt wurde 
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Flower"))
        {
            CollectibleItem item = collision.GetComponent<CollectibleItem>();
            string id = item != null ? item.uniqueId : null;

            TaskCollectibleState.RegisterCollectible(id);
            Debug.Log("Blume eingesammelt! Gesamt: " + TaskCollectibleState.CollectedCount);

            // Zerstört das Objekt, das berührt wurde 
            Destroy(collision.gameObject);
        }
    }
}