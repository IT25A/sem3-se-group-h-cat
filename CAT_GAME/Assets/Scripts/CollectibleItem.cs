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

    [Tooltip("Unique ID for saving/tracking state")]
    public string uniqueId;

    private Collider2D eggCollider;
    private Collider2D collarCollider;

    private void Awake()
    {
        if (string.IsNullOrEmpty(uniqueId))
        {
            uniqueId = System.Guid.NewGuid().ToString();
        }

        eggCollider = GetComponent<Collider2D>();
        collarCollider = GetComponent<Collider2D>();

    }

    private void Start()
    {
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
        // Sobald der Farmer-Dialog startet, schaltet das Skript den Trigger für die Eier live frei
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
            // Restricted collection logic specifically for Wood
            if (type == CollectibleType.Wood && !FireCampTask.isTaskActive)
            {
                return;
            }
            // restricted collection logic specifically for Egg
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