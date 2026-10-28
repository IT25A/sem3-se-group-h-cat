using UnityEngine;
using System.Reflection;

[RequireComponent(typeof(SceneTransition))]
public class NPCDoorLock : MonoBehaviour
{
    [Header("Lock Configuration")]
    [Tooltip("Drag the NPC GameObject here that the player needs to talk to.")]
    public GameObject targetNPC;

    private SceneTransition sceneTransition;
    private bool hasSpokenToNPC = false;
    private bool wasDialogueActive = false;

    void Start()
    {
        // Get the reference to the SceneTransition on this same GameObject (the door)
        sceneTransition = GetComponent<SceneTransition>();

        // Lock the door right at the start
        if (sceneTransition != null)
        {
            sceneTransition.isLocked = true;
        }
    }

    void Update()
    {
        // If it's already unlocked, we don't need to keep checking
        if (hasSpokenToNPC) return;

        if (targetNPC != null && sceneTransition != null)
        {
            // Use Reflection to read 'isDialogueActive' from NPCTalk without modifying its script
            Component npcTalkComponent = targetNPC.GetComponent("NPCTalk");
            if (npcTalkComponent != null)
            {
                FieldInfo activeField = npcTalkComponent.GetType().GetField("isDialogueActive");
                if (activeField != null)
                {
                    bool isDialogueActive = (bool)activeField.GetValue(npcTalkComponent);

                    // Detect the dialogue lifecycle:
                    // 1. The dialogue becomes active (wasDialogueActive turns true)
                    if (isDialogueActive)
                    {
                        wasDialogueActive = true;
                    }
                    // 2. The dialogue was active and now becomes inactive -> Means the conversation ended
                    else if (wasDialogueActive && !isDialogueActive)
                    {
                        hasSpokenToNPC = true;
                        sceneTransition.isLocked = false; // Unlock the door!
                        Debug.Log("NPC dialogue finished! Door unlocked.");
                    }
                }
            }
        }
    }
}