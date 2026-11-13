using UnityEngine;
using System.Collections;

public class AutoDialogueScene3 : MonoBehaviour
{
    [Header("Settings")]
    public NPCTalk targetDialogue;
    public float delay = 0.5f;
    [Tooltip("How long each line stays visible after finishing typing before advancing or closing.")]
    public float pauseBetweenLines = 1.0f; 

    void Start()
    {
        if (targetDialogue != null)
        {
            StartCoroutine(TriggerAndAutoPlayAllLines());
        }    
    }

    IEnumerator TriggerAndAutoPlayAllLines()
    {
        yield return new WaitForSeconds(delay);
        targetDialogue.StartDialogue();

        if (targetDialogue.dialogueData != null)
        {
            // Load language-specific lines to accurately calculate typing duration per line
            int lang = PlayerPrefs.GetInt("Language", 0);
            string[] lines = targetDialogue.dialogueData.dialogueLines;
            
            if (lang == 1 && targetDialogue.dialogueData.germanLines != null && targetDialogue.dialogueData.germanLines.Length > 0)
                lines = targetDialogue.dialogueData.germanLines;
            else if (lang == 2 && targetDialogue.dialogueData.spanishLines != null && targetDialogue.dialogueData.spanishLines.Length > 0)
                lines = targetDialogue.dialogueData.spanishLines;

            if (lines != null && lines.Length > 0)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    // Calculate typing duration from character count and typing speed (+ 0.1s buffer)
                    float typingDuration = (lines[i].Length * targetDialogue.dialogueData.typingSpeed) + 0.1f;
                    yield return new WaitForSeconds(typingDuration + pauseBetweenLines);

                    if (i < lines.Length - 1)
                    {
                        targetDialogue.Interact();
                    }
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
        }

        targetDialogue.EndDialogue();
    }
}