using UnityEngine;

public class QuestDoorScene3 : MonoBehaviour
{
    private SceneTransition sceneTransition;

    [Header("Bedingung")]
    public int requiredFood = 50;

    void Awake()
    {
        sceneTransition = GetComponent<SceneTransition>();
    }

    void Update()
    {
        if (sceneTransition != null)
        {
            if ( SceneData.savedFood >= requiredFood)
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
            if (SceneData.savedFood < requiredFood)
            {
                Debug.Log("Door is locked! Collect all 50 food first.");
            }
        }
    }
}