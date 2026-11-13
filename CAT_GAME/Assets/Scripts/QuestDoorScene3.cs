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
        // Only unlock the transition once the required amount of food has been reached
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