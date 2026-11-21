using UnityEngine;

public class QuestDoorScene3 : MonoBehaviour
{
    private SceneTransition sceneTransition;

    [Header("Condition")]
    public int requiredFood = 50;

    [Tooltip("If enabled, the door ignores food and unlocks only after the dog is at the doctor AND Simba's collar is collected (Scene 4).")]
    public bool requireDogAtDoctorAndCollar = false;

    void Awake()
    {
        sceneTransition = GetComponent<SceneTransition>();
    }

    private bool IsUnlocked()
    {
        if (requireDogAtDoctorAndCollar)
        {
            return SceneData.isDogAtDoctor && TaskCollectibleState.IsCollected("SimbaNecklace");
        }
        return SceneData.savedFood >= requiredFood;
    }

    void Update()
    {
        // Only unlock the transition once the condition is met
        if (sceneTransition != null)
        {
            if (IsUnlocked())
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
            if (!IsUnlocked())
            {
                Debug.Log("Door is locked!");
            }
        }
    }
}