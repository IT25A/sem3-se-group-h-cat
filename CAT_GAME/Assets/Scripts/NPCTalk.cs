using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;
using UnityEngine.SceneManagement;

public class NPCTalk : MonoBehaviour, IInteractable
{
    public static NPCTalk activeTalker;

    [Header("Standard Dialog (Mit Hund, vor Halsband)")]
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText; 
    public Image portraitImage;

    // NEU: Bedingungen für Hund & Halsband
    [Header("Doctor / Quest Conditions (Optional)")]
    public ShadowFollower requiredFollower; 
    [Tooltip("Dialog, wenn Simba OHNE den Hund zum Arzt kommt")]
    public NPCDialogue noDogDialogue; 
    [Tooltip("Dialog, wenn Simba den Hund gebracht hat UND das Halsband hat")]
    public NPCDialogue afterItemDialogue;
    public string requiredCollectibleId = "SimbaNecklace";
    
    [Header("Single Interaction")]
    public bool interactOnlyOnce = false;
    private bool hasCompletedDialogue = false;
   
    private int dialogueIndex;
    private bool isTyping;
    private NPCPatrol patrolScript;
    public bool isDialogueActive;
    public UnityEngine.Events.UnityEvent onDialogueEnded;

    [Header("Egg-Task")]
    public static bool isEggTaskstarted = false;

    // NEU: Speichert die Sätze der aktuell gewählten Sprache
    private string[] activeLines; 
    private NPCDialogue currentDialogue; // Merkt sich den aktuell gewählten Dialog

    public bool CanInteract()
    {
        if (interactOnlyOnce && hasCompletedDialogue)
        {
            return false;
        }
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null)
        {
            return;
        }

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            if (interactOnlyOnce && hasCompletedDialogue)
            {
                return;
            }

            StartDialogue();
        }
    }

    void Awake()
    {
        isEggTaskstarted = false; // Reset the egg task state when the game starts
    }

    void Start()
    {
        patrolScript = GetComponent<NPCPatrol>();
        isEggTaskstarted = false; // Reset the egg task state when the game starts
        if (SceneManager.GetActiveScene().name == "Scene 2") TaskCollectibleState.ResetTaskCollectibles();
    }

    public void StartDialogue()
    {
        if (SceneData.mouseCatched && SceneManager.GetActiveScene().name == "Scene 3") return; 

        // Standardmäßig den normalen Dialog nehmen
        currentDialogue = dialogueData;

        // 1. Prüfen, ob ein Hund verlangt wird, aber NOCH NICHT dabei ist:
        if (requiredFollower != null && !requiredFollower.isFollowing && !requiredFollower.hasStayedAtDoctor)
        {
            if (noDogDialogue != null)
            {
                currentDialogue = noDogDialogue;
            }
            else
            {
                return; // Falls kein Ohne-Hund-Dialog zugewiesen ist, nichts tun
            }
        }
        // 2. Wenn der Hund dabei ist (oder schon abgegeben wurde) UND das Halsband eingesammelt ist:
        else if (afterItemDialogue != null && TaskCollectibleState.IsCollected(requiredCollectibleId))
        {
            currentDialogue = afterItemDialogue;
        }
        
        activeTalker = this;
        isDialogueActive = true;
        isEggTaskstarted = true;

        if (patrolScript != null)
        {
            patrolScript.isInteracting = true;
        }

        dialogueIndex = 0;
        nameText.SetText(currentDialogue.npcName);
        portraitImage.sprite = currentDialogue.npcPortrait;
        
        int lang = PlayerPrefs.GetInt("Language", 0);
        if (lang == 0) activeLines = currentDialogue.dialogueLines;
        else if (lang == 1) activeLines = currentDialogue.germanLines;
        else if (lang == 2) activeLines = currentDialogue.spanishLines;

        if (activeLines == null || activeLines.Length == 0)
        {
            activeLines = currentDialogue.dialogueLines;
        }

        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(activeLines[dialogueIndex]);
            isTyping = false;
        }
        else if (dialogueIndex + 1 < activeLines.Length)
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

        foreach (char letter in activeLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(currentDialogue.typingSpeed);
        }
        isTyping = false;
        
        if (currentDialogue.autoProgressLines != null && 
            currentDialogue.autoProgressLines.Length > dialogueIndex && 
            currentDialogue.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(currentDialogue.autoProgressDelay);
            NextLine();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        if (activeTalker == this)
        {
            activeTalker = null;
        }
        isDialogueActive = false;
        hasCompletedDialogue = true;

        if (patrolScript != null)
        {
            patrolScript.isInteracting = false;
        }

        dialogueText.SetText("");
        dialoguePanel.SetActive(false);

        onDialogueEnded?.Invoke();
    }
}