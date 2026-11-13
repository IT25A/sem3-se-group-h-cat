using UnityEngine;

[RequireComponent(typeof(SceneTransition))]
public class QuestDoor : MonoBehaviour
{
    private SceneTransition sceneTransition;

    private void Awake()
    {
        sceneTransition = GetComponent<SceneTransition>();
    }

    private void Update()
    {
        // Only unlock the door once the flower quest is completed and at least 10 food has been collected
        if (FlowerQuestManager.Instance != null && FlowerQuestManager.Instance.isQuestCompleted && SceneData.savedFood >= 10)
        {
            if (sceneTransition != null)
            {
                sceneTransition.isLocked = false;
            }
        }
        else
        {
            if (sceneTransition != null)
            {
                sceneTransition.isLocked = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (FlowerQuestManager.Instance == null || !FlowerQuestManager.Instance.isQuestCompleted)
            {
                Debug.Log("Door is locked! Collect all 10 flowers first.");
            }
        }
    }
}