using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public enum CollectibleType
    {
        Egg,
        Flower,
        Mouse,
        Key,
        SimbaNecklace,
        Wood
    }

    [Header("Collectible Settings")]
    public CollectibleType type;

    [Tooltip("Unique ID to persist collection state across scene transitions")]
    public string uniqueId;

    private Collider2D eggCollider;
    private Collider2D collarCollider;

    private void Awake()
    {
        // Fallback-ID generieren, falls im Inspector keine vergeben wurde
        if (string.IsNullOrEmpty(uniqueId))
        {
            uniqueId = System.Guid.NewGuid().ToString();
        }

        eggCollider = GetComponent<Collider2D>();
        collarCollider = GetComponent<Collider2D>();

    }

    private void Start()
    {
        // Bereits eingesammelte Gegenstände beim Laden der Szene ausblenden
        if (TaskCollectibleState.IsCollected(uniqueId))
        {
            gameObject.SetActive(false);
        }
        if (type == CollectibleType.Egg)
        {
            eggCollider.isTrigger = NPCTalk.isEggTaskstarted;
        }
        if (type == CollectibleType.SimbaNecklace && collarCollider != null)
        {
            collarCollider.isTrigger = NPCTalk.isCollarTaskstarted;
        }
    }

    private void Update()
    {
        // Schaltet den Trigger für Eier und Halsband erst frei, sobald die jeweilige Quest im Dialog gestartet wurde
        if (type == CollectibleType.Egg && !eggCollider.isTrigger)
        {
            eggCollider.isTrigger = NPCTalk.isEggTaskstarted;
        }
        if (type == CollectibleType.SimbaNecklace && collarCollider != null && !collarCollider.isTrigger)
        {
            collarCollider.isTrigger = NPCTalk.isCollarTaskstarted;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Player entered trigger for {type} collectible with ID: {uniqueId}");
        if (other.CompareTag("Player"))
        {
            // Holz, Eier und Halsband dürfen erst nach Start der zugehörigen Quest eingesammelt werden
            if (type == CollectibleType.Wood && !FireCampTask.isTaskActive)
            {
                return;
            }
            if (type == CollectibleType.Egg)
            {
                if (!NPCTalk.isEggTaskstarted)
                {
                    Debug.Log("Erst mit Farmer talken");
                    return;
                }
            }

            if (type == CollectibleType.SimbaNecklace)
            {
                if (!NPCTalk.isCollarTaskstarted)
                {
                    Debug.Log("Erst mit Arzt talken");
                    return;
                }
                else
                {
                    Debug.Log("Simba's Halsband kann eingesammelt werden");
                }
            }

            Collect();
        }
    }

    private void Collect()
    {
        switch (type)
        {
            case CollectibleType.Egg:
                TaskCollectibleState.RegisterEgg(uniqueId);
                break;
            case CollectibleType.Flower:
                TaskCollectibleState.RegisterFlower(uniqueId);
                break;
            case CollectibleType.Mouse:
                TaskCollectibleState.RegisterMouse(uniqueId);
                break;
            case CollectibleType.Key:
                TaskCollectibleState.RegisterKey(uniqueId);
                break;
            case CollectibleType.SimbaNecklace:
                TaskCollectibleState.RegisterSimbaNecklace(uniqueId);
                break;
            case CollectibleType.Wood:
                TaskCollectibleState.RegisterWood(uniqueId);
                break;
        }

        gameObject.SetActive(false);
    }
}