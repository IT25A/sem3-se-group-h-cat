using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPCChoiceTalk : MonoBehaviour, IInteractable
{
    [Header("Dialog Daten")]
    public NPCDialogue dialogueData;

    [Header("UI Elemente (Panel 2)")]
    public GameObject dialoguePanel; // Hier Panel 2 reinziehen
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;

    [Header("Choice Buttons")]
    public GameObject[] choiceButtonObjects; // Die 3 GameObjects der Buttons
    public TMP_Text[] choiceButtonTexts;     // Die 3 Text-Komponenten der Buttons

    private int dialogueIndex;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private bool isWaitingForChoice = false;

    void Start()
    {
        // Versteckt das Panel beim Start
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    // Wird vom PlayerController aufgerufen, wenn Simba "E" drückt
    public void Interact()
    {
        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else if (!isTyping && !isWaitingForChoice) 
        {
            // Nur weiterklicken, wenn der Text fertig ist UND keine Auswahl ansteht
            NextLine();
        }
    }
    public bool CanInteract()
    {
        return true; // Sagt dem Player, dass er jederzeit mit dem NPC interagieren darf
    }

    public void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;
        isWaitingForChoice = false;

        // UI füllen
        if (nameText != null) nameText.text = dialogueData.npcName;
        if (portraitImage != null && dialogueData.npcPortrait != null) 
            portraitImage.sprite = dialogueData.npcPortrait;

        // Alle Buttons zur Sicherheit erstmal ausblenden
        HideAllChoices();

        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        dialogueIndex++;

        if (dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        // Text Buchstabe für Buchstabe tippen
        foreach (char c in dialogueData.dialogueLines[dialogueIndex].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        
        // Sobald der Satz fertig getippt ist, prüfen wir, ob Buttons auftauchen sollen!
        ShowChoicesIfAny();
    }

    private void ShowChoicesIfAny()
    {
        // Prüfen, ob für den aktuellen Satz (dialogueIndex) Entscheidungen hinterlegt sind
        foreach (DialogueChoice choice in dialogueData.choices)
        {
            if (choice.dialogueIndex == dialogueIndex)
            {
                isWaitingForChoice = true; // Blockiert das Weiterklicken mit "E"

                // Buttons aktivieren und befüllen
                for (int i = 0; i < choice.choices.Length; i++)
                {
                    if (i < choiceButtonObjects.Length)
                    {
                        choiceButtonObjects[i].SetActive(true);
                        choiceButtonTexts[i].text = choice.choices[i];

                        // Klick-Befehl für den Button setzen
                        int nextIndex = choice.nextDialogueIndexes[i];
                        Button btnComp = choiceButtonObjects[i].GetComponent<Button>();
                        
                        btnComp.onClick.RemoveAllListeners();
                        btnComp.onClick.AddListener(() => MakeChoice(nextIndex));
                    }
                }
                break; // Wir haben die Auswahl gefunden, Schleife beenden
            }
        }
    }

    public void MakeChoice(int nextIndex)
    {
        isWaitingForChoice = false;
        HideAllChoices();

        // Simba hat geklickt -> Wir springen zum neuen Satz und tippen ihn
        dialogueIndex = nextIndex;
        StartCoroutine(TypeLine());
    }

    private void HideAllChoices()
    {
        foreach (GameObject btn in choiceButtonObjects)
        {
            if (btn != null) btn.SetActive(false);
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        HideAllChoices();
    }
}