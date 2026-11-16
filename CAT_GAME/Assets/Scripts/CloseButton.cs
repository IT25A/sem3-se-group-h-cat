using UnityEngine;

public class CloseButton : MonoBehaviour
{
    // Ends all currently active NPC dialogues in the scene
    public void Close()
    {
        NPCTalk[] npcs = FindObjectsByType<NPCTalk>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        foreach (NPCTalk npc in npcs)
        {
         if (npc.isDialogueActive)
         {
             npc.EndDialogue();
         }
        }
    }
}
