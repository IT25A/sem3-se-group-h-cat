using System.Collections;
using UnityEngine;

public class FlowerQuestCheck : MonoBehaviour
{

    public NPCDialogue startDialog;     // Startdialog: Auftrag zum Sammeln von 10 Blumen
    public NPCDialogue inbetweenDialog; // Zwischendialog: Noch nicht genug Blumen gesammelt
    public NPCDialogue finalDialog;     // Abschlussdialog: Alle 10 Blumen wurden gesammelt

    [Header("Reward")]
    public int reward = 10;
    private NPCTalk npcTalk;
    private bool dialogueActive = false;

    void Start()
    {
        npcTalk = GetComponent<NPCTalk>();
        npcTalk.dialogueData = startDialog;
    }

    void Update()
    {
        // Erkennt den Moment, in dem der Abschlussdialog beendet wird, um einmalig die Belohnung zu vergeben
        if (dialogueActive && !npcTalk.isDialogueActive)
        {
            if (npcTalk.dialogueData == finalDialog && SceneData.flowerQuestCompleted == false)
            {
                if (FlowerQuestManager.Instance != null)
                {
                    FlowerQuestManager.Instance.finalDialogueFinished = true;
                }

                if (FoodManager.Instance != null)
                {
                    FoodManager.Instance.AddFood(reward);
                    Debug.Log($"[Quest] {reward} Food hinzugefügt!");
                    SceneData.flowerQuestCompleted = true;
                }
            }
        }

        dialogueActive = npcTalk.isDialogueActive;
        if (FlowerQuestManager.Instance != null)
        {
            int current = FlowerQuestManager.Instance.currentFlowerCount;
            bool finished = FlowerQuestManager.Instance.finalDialogueFinished;

        }

        // Dialogdaten nur bei geschlossenem Dialogfenster aktualisieren, um Textwechsel während des Gesprächs zu verhindern
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