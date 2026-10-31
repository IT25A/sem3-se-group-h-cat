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

        // BEHOBEN: Einheitlich auf Standard 1 (oder 0, je nachdem was du willst, aber passend zu NPCTalk)
        int lang = PlayerPrefs.GetInt("Language", 1);
        
        if (lang == 0) activeLines = dialogueData.dialogueLines;
        else if (lang == 1) activeLines = dialogueData.germanLines;
        else if (lang == 2) activeLines = dialogueData.spanishLines;

        // Sicherheitsnetz: Falls gewählte Sprache leer ist, Fallback auf Englisch
        if (activeLines == null || activeLines.Length == 0)
        {
            activeLines = dialogueData.dialogueLines;
        }

        if (activeLines == null || activeLines.Length == 0)
        {
            isDialogueActive = false;
            return;
        }

        HideAllChoices();
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private void NextLine()
    {
        dialogueIndex++;

        if (activeLines != null && dialogueIndex < activeLines.Length)
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

        if (activeLines == null || dialogueIndex >= activeLines.Length)
        {
            isTyping = false;
            EndDialogue();
            yield break;
        }

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
        if (dialogueData.choices == null) return;

        foreach (DialogueChoice choice in dialogueData.choices)
        {
            if (choice.dialogueIndex == dialogueIndex)
            {
                isWaitingForChoice = true; 
                HideAllChoices();
                
                int lang = PlayerPrefs.GetInt("Language", 1);
                string[] activeChoices = choice.choices; 
                
                if (lang == 0) activeChoices = choice.choices;
                else if (lang == 1) activeChoices = choice.germanChoices;
                else if (lang == 2) activeChoices = choice.spanishChoices;

                if (activeChoices == null || activeChoices.Length == 0)
                {
                    activeChoices = choice.choices;
                }
                
                for (int i = 0; i < activeChoices.Length; i++)
                {
                    if (i < choiceButtonObjects.Length && choiceButtonObjects[i] != null)
                    {
                        choiceButtonObjects[i].SetActive(true);
                        if (i < choiceButtonTexts.Length && choiceButtonTexts[i] != null)
                        {
                            choiceButtonTexts[i].text = activeChoices[i];
                        }

                        int nextIndex = 0;
                        if (choice.nextDialogueIndexes != null && i < choice.nextDialogueIndexes.Length)
                        {
                            nextIndex = choice.nextDialogueIndexes[i];
                        }

                        Button btnComp = choiceButtonObjects[i].GetComponent<Button>();
                        if (btnComp != null)
                        {
                            btnComp.onClick.RemoveAllListeners();
                            int indexToPass = nextIndex; 
                            btnComp.onClick.AddListener(() => MakeChoice(indexToPass));
                        }
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

        // SICHERHEIT: Prüfen ob der Ziel-Index existiert, sonst Dialog beenden
        if (activeLines == null || nextIndex < 0 || nextIndex >= activeLines.Length)
        {
            Debug.LogWarning("Choice führt zu ungültigem Index: " + nextIndex);
            EndDialogue();
            return;
        }

        dialogueIndex = nextIndex;
        StartCoroutine(TypeLine());
    }

    private void HideAllChoices()
    {
        if (choiceButtonObjects == null) return;
        foreach (GameObject btn in choiceButtonObjects)
        {
            if (btn != null) btn.SetActive(false);
        }
    }

    private void EndDialogue()
    {
        isDialogueActive = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        HideAllChoices();
    }
}