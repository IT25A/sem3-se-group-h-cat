using System.Collections;
using UnityEngine;

public class FlowerQuestCheck : MonoBehaviour
{

    public NPCDialogue startDialog;       // 1. "Sammle 10 Blumen!"
    public NPCDialogue inbetweenDialog; // 2. "Du hast erst x Blumen..."
    public NPCDialogue finalDialog;      // 3. "Klasse, geh zur Treppe!"

    [Header("Reward")]
    public int reward = 10;
    private bool RewardGiven = false;
    private NPCTalk npcTalk;
    private bool Taskgiven = false; // Gedächtnis der Katze
    private bool dialogueActive = false;

    void Start()
    {
        npcTalk = GetComponent<NPCTalk>();

        // Ganz am Anfang legen wir den 1. Dialog ein
        npcTalk.dialogueData = startDialog;
    }

    void Update()
    {
        // SICHERHEITS-CHECK: Wird gerade gesprochen?
        if (dialogueActive && !npcTalk.isDialogueActive)
        {
            // Wenn Simba gerade den allerersten Text liest, merkt sich das Skript
            if (npcTalk.dialogueData == finalDialog && !RewardGiven)
            {
                RewardGiven = true;

                if (FlowerQuestManager.Instance != null)
                {
                    FlowerQuestManager.Instance.finalDialogueFinished = true;
                }

                // Wir brechen hier ab, damit der Text nicht mitten im Sprechen ausgetauscht wird
                if (FoodManager.Instance != null)
                {
                    FoodManager.Instance.AddFood(reward);
                    Debug.Log($"[Quest] {reward} Food hinzugefügt!");
                }
            }
        }

        dialogueActive = npcTalk.isDialogueActive;
        if (FlowerQuestManager.Instance != null)
        {
            int current = FlowerQuestManager.Instance.currentFlowerCount;
            bool finished = FlowerQuestManager.Instance.finalDialogueFinished;

        }

        // Wenn das Fenster GESCHLOSSEN ist, bereiten wir den Text für das nächste Ansprechen vor:
        if (npcTalk.isDialogueActive)
        {
            return;
        }
        if (FlowerQuestManager.Instance != null)
        {
            if (FlowerQuestManager.Instance.currentFlowerCount >= 10)
            {
                npcTalk.dialogueData = finalDialog;
            }
            else
            {
                npcTalk.dialogueData = inbetweenDialog;
            }
        }
    }
}