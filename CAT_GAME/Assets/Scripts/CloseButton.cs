using UnityEngine;

public class CloseButton : MonoBehaviour
{
    // Beendet alle aktuell geöffneten NPC-Dialoge in der Szene
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
