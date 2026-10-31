using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;

public class NPCTalk : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText; 
    public Image portraitImage;
   
    private int dialogueIndex;
    private bool isTyping;
    private NPCPatrol patrolScript;
    public bool isDialogueActive;

    // NEU: Speichert die Sätze der aktuell gewählten Sprache
    private string[] activeLines; 

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null){
            return;
        }
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void Start()
    {
        patrolScript = GetComponent<NPCPatrol>();
    }

    public void StartDialogue()
    {
        isDialogueActive = true;

        if (patrolScript != null)
        {
            patrolScript.isInteracting = true;
        }

        dialogueIndex = 0;
        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;
        
        // NEU: Sprache auslesen (0=EN, 1=DE, 2=ES)
        int lang = PlayerPrefs.GetInt("Language", 1);
        if (lang == 0) activeLines = dialogueData.dialogueLines;
        else if (lang == 1) activeLines = dialogueData.germanLines;
        else if (lang == 2) activeLines = dialogueData.spanishLines;

        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            // NEU: Nutzt activeLines
            dialogueText.SetText(activeLines[dialogueIndex]);
            isTyping = false;
        }
        else if (dialogueIndex + 1 < activeLines.Length) // NEU: Nutzt activeLines.Length
        {
            dialogueIndex++;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        // NEU: Nutzt activeLines zum Tippen
        foreach (char letter in activeLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }
        isTyping = false;
        
        if (dialogueData.autoProgressLines.Length > dialogueIndex 
        && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;

        if (patrolScript != null)
        {
            patrolScript.isInteracting = false;
        }

        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
    }
}