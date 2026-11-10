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

    private void Awake()
    {
        if (string.IsNullOrEmpty(uniqueId))
        {
            uniqueId = System.Guid.NewGuid().ToString();
        }

        eggCollider = GetComponent<Collider2D>();

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
    }

    private void Update()
    {
        // Sobald der Farmer-Dialog startet, schaltet das Skript den Trigger für die Eier live frei
        if (type == CollectibleType.Egg && !eggCollider.isTrigger)
        {
            eggCollider.isTrigger = NPCTalk.isEggTaskstarted;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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