using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;


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

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        //if no dialogue data or game is paused and dialogue is not active, return
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
        dialoguePanel.SetActive(true);
        //PauseController.SetPaused(true);

        StartCoroutine(TypeLine());
    
    }
    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (dialogueIndex + 1 < dialogueData.dialogueLines.Length)
        {
            dialogueIndex++;
            //If another line, type next line
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

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
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
        //PauseController.SetPaused(false);
    }

}    
