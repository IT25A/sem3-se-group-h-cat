using UnityEngine;

public class CloseButton : MonoBehaviour
{
public void Close()
    {
     
     //Findet alle NPCTalk-Skripte
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
