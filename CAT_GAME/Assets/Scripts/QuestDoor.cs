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
        if (FlowerQuestManager.Instance != null && FlowerQuestManager.Instance.isQuestCompleted)
        {
            if (sceneTransition != null)
            {
                sceneTransition.isLocked = false; // Unlocked!
            }
        }
        else
        {
            if (sceneTransition != null)
            {
                sceneTransition.isLocked = true; // Locked!
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