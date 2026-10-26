using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Egg"))
        {
            CollectibleItem item = collision.GetComponent<CollectibleItem>();
            string id = item != null ? item.uniqueId : null;

            TaskCollectibleState.RegisterEgg(id);
            Debug.Log("Ei eingesammelt! Eier gesamt: " + TaskCollectibleState.EggCount);

            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Flower"))
        {
            CollectibleItem item = collision.GetComponent<CollectibleItem>();
            string id = item != null ? item.uniqueId : null;

            TaskCollectibleState.RegisterFlower(id);
            Debug.Log("Blume eingesammelt! Blumen gesamt: " + TaskCollectibleState.FlowerCount);

            Destroy(collision.gameObject);
        }
    }
}