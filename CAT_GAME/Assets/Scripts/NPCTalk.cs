using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Reflection;
using UnityEngine.SceneManagement;

public class NPCTalk : MonoBehaviour, IInteractable
{
    public static NPCTalk activeTalker;

    [Header("Default Dialogue (With Dog, Before Collar)")]
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    [Header("Doctor / Quest Conditions (Optional)")]
    public ShadowFollower requiredFollower;
    [Tooltip("Dialogue when Simba visits the doctor WITHOUT the dog")]
    public NPCDialogue noDogDialogue;
    [Tooltip("Dialogue when Simba has brought the dog AND collected the collar")]
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

    [Header("Collar-Task")]
    public static bool isCollarTaskstarted = false;

    private string[] activeLines;
    private NPCDialogue currentDialogue;

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
        isEggTaskstarted = false;
    }

    void Start()
    {
        patrolScript = GetComponent<NPCPatrol>();
        isCollarTaskstarted = false;
        if (SceneManager.GetActiveScene().name == "Scene 2") TaskCollectibleState.ResetTaskCollectibles();
    }

    public void StartDialogue()
    {
        currentDialogue = dialogueData;

        // 1. If the injured dog is required, but has not followed the player yet:
        if (requiredFollower != null && !requiredFollower.isFollowing && !requiredFollower.hasStayedAtDoctor)
        {
            if (noDogDialogue != null)
            {
                currentDialogue = noDogDialogue;
            }
            else
            {
                return;
            }
        }
        // 2. If the dog is already at the doctor and the collar has been found:
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

        // After the first conversation with the dog, the dog stays with the doctor and the collar quest is unlocked
        if (requiredFollower != null && currentDialogue == dialogueData)
        {
            requiredFollower.hasStayedAtDoctor = true;
            requiredFollower.isFollowing = false;
            isCollarTaskstarted = true;
        }

        dialogueText.SetText("");
        dialoguePanel.SetActive(false);

        onDialogueEnded?.Invoke();
    }

    public bool IsTalking()
    {
        return isDialogueActive;
    }
    public bool IsPlayingAfterItemDialogue()
    {
        return isDialogueActive && currentDialogue != null && currentDialogue == afterItemDialogue;
    }
}