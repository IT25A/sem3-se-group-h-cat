using UnityEngine;

public class QuestDoorScene3 : MonoBehaviour
{
    private SceneTransition sceneTransition;

    [Header("Condition")]
    public int requiredFood = 50;

    void Awake()
    {
        sceneTransition = GetComponent<SceneTransition>();
    }

    void Update()
    {
        // Übergang erst freigeben, wenn die erforderliche Futtermenge erreicht wurde
        if (sceneTransition != null)
        {
            if ( SceneData.savedFood >= requiredFood)
            {
                sceneTransition.isLocked = false;
            }
            else
            {
                sceneTransition.isLocked = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (SceneData.savedFood < requiredFood)
            {
                Debug.Log("Door is locked! Collect all 50 food first.");
            }
        }
    }
}