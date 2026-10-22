using UnityEngine;

public class FlowerQuestCheck : MonoBehaviour
{
    
    public NPCDialogue startDialog;       // 1. "Sammle 10 Blumen!"
    public NPCDialogue inbetweenDialog; // 2. "Du hast erst x Blumen..."
    public NPCDialogue finalDialog;      // 3. "Klasse, geh zur Treppe!"

    private NPCTalk npcTalk;
    private bool Taskgiven = false; // Gedächtnis der Katze

    void Start()
    {
        npcTalk = GetComponent<NPCTalk>();
        
        // Ganz am Anfang legen wir den 1. Dialog ein
        npcTalk.dialogueData = startDialog;
    }

    void Update()
    {
        // SICHERHEITS-CHECK: Wird gerade gesprochen?
        if (npcTalk.isDialogueActive)
        {
            // Wenn Simba gerade den allerersten Text liest, merkt sich das Skript
            if (npcTalk.dialogueData == startDialog)
            {
                Taskgiven = true;
            }
            
            // Wir brechen hier ab, damit der Text nicht mitten im Sprechen ausgetauscht wird
            return; 
        }

        // Wenn das Fenster GESCHLOSSEN ist, bereiten wir den Text für das nächste Ansprechen vor:
        if (FlowerQuestManager.Instance != null && FlowerQuestManager.Instance.currentFlowerCount >= 10)
        {
            // Zustand 3: Er hat 10 Blumen
            npcTalk.dialogueData = finalDialog;
        }
        else if (Taskgiven)
        {
            // Zustand 2: Er hat die Aufgabe bekommen, aber noch keine 10 Blumen
            npcTalk.dialogueData = inbetweenDialog;
        }
    }
}