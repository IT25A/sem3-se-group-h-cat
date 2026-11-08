using UnityEngine;

public class FireCampTask : MonoBehaviour
{
    [Header("Reward Configuration")]
    public int reward = 10;
    public int requiredWood = 10;

    [Header("Visual Elements")]
    public GameObject campfireVisual;

    // Global state checked by CollectibleItem.cs
    public static bool isTaskActive = false;

    private bool rewardGiven = false;
    private bool wasInDialogue = false;
    private NPCChoiceTalk npcTalk;

    void Awake()
    {
        // Fuerza a que la misión inicie cerrada al entrar a Play Mode
        isTaskActive = false;
    }

    void Start()
    {
        npcTalk = GetComponent<NPCChoiceTalk>();
        if (npcTalk == null)
        {
            Debug.LogError("[FireCampTask] ERROR: No NPCChoiceTalk found on this object!");
        }

        if (campfireVisual != null)
        {
            campfireVisual.SetActive(false);
        }
    }

    void Update()
    {
        if (npcTalk == null || rewardGiven) return;

        if (npcTalk.IsTalking())
        {
            wasInDialogue = true;
        }
        else if (wasInDialogue)
        {
            wasInDialogue = false;

            // Paso 1: Si la misión aún NO está activa, revisamos si aceptó
            if (!isTaskActive)
            {
                // AQUÍ VALIDAMOS LA OPCIÓN: 
                // Si el método de tu compañero te da el índice de la elección (ej: npcTalk.GetLastChoice() == 0)
                // O si simplemente al cerrar el diálogo quieres verificar si dio "Sí":
                // (Si tu NPCChoiceTalk usa otra función para saber si aceptó, cámbiala en este if).

                // Si quieres que se active solo si eligió la opción de aceptar la misión:
                // if (npcTalk.GetChosenOption() == 0) // <--- Descomenta si tu compañero tiene este método

                // Por mientras, activamos el flag:
                isTaskActive = true;
                Debug.Log("[FireCampTask] Task activated: Simba can now pick up wood.");
                return;
            }

            // Paso 2: Si ya estaba activa, revisamos si completó la madera
            if (TaskCollectibleState.WoodCount >= requiredWood)
            {
                rewardGiven = true;

                if (campfireVisual != null)
                {
                    campfireVisual.SetActive(true);
                }

                if (FoodManager.Instance != null)
                {
                    FoodManager.Instance.AddFood(reward);
                    Debug.Log($"[FireCampTask] Campfire lit! Added {reward} points.");
                }
                else
                {
                    Debug.LogError("[FireCampTask] FoodManager.Instance is NULL!");
                }

                TaskCollectibleState.ResetTaskCollectibles();
            }
            else
            {
                Debug.Log($"[FireCampTask] In progress: {TaskCollectibleState.WoodCount}/{requiredWood} wood collected.");
            }
        }
    }
}