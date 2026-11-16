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

        // Lock the door by default on start
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
            // Reads 'isDialogueActive' via reflection from NPCTalk to monitor the conversation state
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
                    // Permanently unlock the door as soon as the previously active dialogue has ended
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