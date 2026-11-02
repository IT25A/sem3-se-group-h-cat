using UnityEngine;

public class QuestDoorScene2 : MonoBehaviour
{
    private SceneTransition sceneTransition;

    [Header("Bedingung")]
    public int requiredEggs = 4;
    public int requiredFood = 25;

    void Awake()
    {
        sceneTransition = GetComponent<SceneTransition>();
    }

    void Update()
    {
        if (sceneTransition != null)
        {
            if (TaskCollectibleState.EggCount >= requiredEggs || SceneData.savedFood >= requiredFood)
            {
                sceneTransition.isLocked = false; // Tor öffnen
            }
            else
            {
                sceneTransition.isLocked = true;  // Tor schließen
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (TaskCollectibleState.EggCount < requiredEggs && SceneData.savedFood < requiredFood)
            {
                Debug.Log("Door is locked! Collect all 4 eggs and 25 food first.");
            }
        }
    }
}