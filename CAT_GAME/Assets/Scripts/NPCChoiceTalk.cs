using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPCChoiceTalk : MonoBehaviour, IInteractable
{
    [Header("Dialog Daten")]
    public NPCDialogue dialogueData;

    [Header("UI Elemente (Panel 2)")]
    public GameObject dialoguePanel; 
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;

    [Header("Choice Buttons")]
    public GameObject[] choiceButtonObjects; 
    public TMP_Text[] choiceButtonTexts;     

    private int dialogueIndex;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private bool isWaitingForChoice = false;

    private string[] activeLines; 

    void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else if (!isTyping && !isWaitingForChoice) 
        {
            NextLine();
        }
    }

    public bool CanInteract()
    {
        return true; 
    }

    public void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0;
        isWaitingForChoice = false;

        if (nameText != null) nameText.text = dialogueData.npcName;
        if (portraitImage != null && dialogueData.npcPortrait != null) 
            portraitImage.sprite = dialogueData.npcPortrait;

        // Sprache auslesen (Der doppelte Eintrag wurde hier entfernt)
        int lang = PlayerPrefs.GetInt("Language", 1);
        Debug.Log("Der NPC liest Sprach-ID: " + lang);

        if (lang == 0) activeLines = dialogueData.dialogueLines;
        else if (lang == 1) activeLines = dialogueData.germanLines;
        else if (lang == 2) activeLines = dialogueData.spanishLines;

        HideAllChoices();
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        dialogueIndex++;

        if (dialogueIndex < activeLines.Length)
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

        foreach (char c in activeLines[dialogueIndex].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
        ShowChoicesIfAny();
    }

    private void ShowChoicesIfAny()
    {
        foreach (DialogueChoice choice in dialogueData.choices)
        {
            if (choice.dialogueIndex == dialogueIndex)
            {
                isWaitingForChoice = true; 
                
                int lang = PlayerPrefs.GetInt("Language", 1);
                string[] activeChoices = choice.choices; 
                
                // Hier wurde der Fehler behoben: Buttons nutzen jetzt die richtige Sprache
                if (lang == 0) activeChoices = choice.choices;
                else if (lang == 1) activeChoices = choice.germanChoices;
                else if (lang == 2) activeChoices = choice.spanishChoices;
                
                for (int i = 0; i < activeChoices.Length; i++)
                {
                    if (i < choiceButtonObjects.Length)
                    {
                        choiceButtonObjects[i].SetActive(true);
                        choiceButtonTexts[i].text = activeChoices[i];

                        int nextIndex = choice.nextDialogueIndexes[i];
                        Button btnComp = choiceButtonObjects[i].GetComponent<Button>();
                        
                        btnComp.onClick.RemoveAllListeners();
                        btnComp.onClick.AddListener(() => MakeChoice(nextIndex));
                    }
                }
                break; 
            }
        }
    }

    public void MakeChoice(int nextIndex)
    {
        isWaitingForChoice = false;
        HideAllChoices();

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