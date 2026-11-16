using UnityEngine;
using System.Collections;

public class AutoDialogueScene3 : MonoBehaviour
{
    [Header("Einstellungen")]
    public NPCTalk targetDialogue;
    public float delay = 0.5f;
    [Tooltip("Wie lange soll jede Zeile nach dem Fertigtippen stehen bleiben, bevor die nächste kommt / geschlossen wird?")]
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
        // 1. Warten bis der Dialog starten soll (0.5s)
        yield return new WaitForSeconds(delay);
        
        // 2. Erste Zeile starten
        targetDialogue.StartDialogue();

        if (targetDialogue.dialogueData != null)
        {
            // Aktuelle Sprache herausfinden
            int lang = PlayerPrefs.GetInt("Language", 0);
            string[] lines = targetDialogue.dialogueData.dialogueLines;
            
            if (lang == 1 && targetDialogue.dialogueData.germanLines != null && targetDialogue.dialogueData.germanLines.Length > 0)
                lines = targetDialogue.dialogueData.germanLines;
            else if (lang == 2 && targetDialogue.dialogueData.spanishLines != null && targetDialogue.dialogueData.spanishLines.Length > 0)
                lines = targetDialogue.dialogueData.spanishLines;

            if (lines != null && lines.Length > 0)
            {
                // 3. Jede Zeile durchgehen
                for (int i = 0; i < lines.Length; i++)
                {
                    // Exakte Tippdauer für die aktuelle Zeile (i) berechnen + 0.1s kleiner Puffer
                    float typingDuration = (lines[i].Length * targetDialogue.dialogueData.typingSpeed) + 0.1f;

                    // Warten, bis die Zeile fertig getippt ist + 1 Sekunde Lesezeit
                    yield return new WaitForSeconds(typingDuration + pauseBetweenLines);

                    // Wenn es NICHT die letzte Zeile ist -> nächste Zeile starten
                    if (i < lines.Length - 1)
                    {
                        targetDialogue.Interact(); // Springt automatisch zur nächsten Zeile (NextLine)
                    }
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
        }

        // 4. Nach der letzten Zeile (+ 1 Sekunde) Dialogbox schließen
        targetDialogue.EndDialogue();
    }
}