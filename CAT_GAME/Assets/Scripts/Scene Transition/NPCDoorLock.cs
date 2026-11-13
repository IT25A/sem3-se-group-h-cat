using UnityEngine;
using System.Reflection;

[RequireComponent(typeof(SceneTransition))]
public class NPCDoorLock : MonoBehaviour
{
    [Header("Lock Configuration")]
    [Tooltip("NPC GameObject the player must talk to in order to unlock this door")]
    public GameObject targetNPC;

    private SceneTransition sceneTransition;
    private bool hasSpokenToNPC = false;
    private bool wasDialogueActive = false;

    void Start()
    {
        sceneTransition = GetComponent<SceneTransition>();

        // Tür beim Start standardmäßig sperren
        if (sceneTransition != null)
        {
            sceneTransition.isLocked = true;
        }
    }

    void Update()
    {
        if (hasSpokenToNPC) return;

        if (targetNPC != null && sceneTransition != null)
        {
            // Liest 'isDialogueActive' per Reflection aus NPCTalk, um den Gesprächsstatus zu überwachen
            Component npcTalkComponent = targetNPC.GetComponent("NPCTalk");
            if (npcTalkComponent != null)
            {
                FieldInfo activeField = npcTalkComponent.GetType().GetField("isDialogueActive");
                if (activeField != null)
                {
                    bool isDialogueActive = (bool)activeField.GetValue(npcTalkComponent);

                    if (isDialogueActive)
                    {
                        wasDialogueActive = true;
                    }
                    // Sobald der zuvor aktive Dialog beendet wurde, wird die Tür dauerhaft entsperrt
                    else if (wasDialogueActive && !isDialogueActive)
                    {
                        hasSpokenToNPC = true;
                        sceneTransition.isLocked = false;
                        Debug.Log("NPC dialogue finished! Door unlocked.");
                    }
                }
            }
        }
    }
}