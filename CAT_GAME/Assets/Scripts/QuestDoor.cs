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
        // Tür erst freigeben, wenn die Blumen-Quest abgeschlossen ist und mindestens 10 Futter gesammelt wurden
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