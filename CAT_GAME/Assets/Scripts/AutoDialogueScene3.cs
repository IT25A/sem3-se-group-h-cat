using System.IO.Pipes;
using UnityEngine;
using System.Collections;

public class AutoDialogueScene3 : MonoBehaviour
{
    //NPC-Skript verknüpfen
    public NPCTalk targetDialogue;
    public float delay = 0.5f;

    void Start()
    {
        if (targetDialogue !=null)
        {
            //Coroutine = Methode in Unity, die Pausen einlegen kann
            StartCoroutine(TriggerDialogue());
        }    
    }
    IEnumerator TriggerDialogue()
    {
        //Pause
        yield return new WaitForSeconds(delay);
        //greifen auf das NPCTalk-Skript zu und rufen die Methode StartDialogue auf
        targetDialogue.StartDialogue();
    }
}
