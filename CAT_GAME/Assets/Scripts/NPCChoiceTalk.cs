using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NPCChoiceTalk : MonoBehaviour, IInteractable
{
    [Header("Dialogue Data")]
    public NPCDialogue dialogueData;
    public NPCDialogue AfterMouse;
    public NPCDialogue KeepGoingDialogue;
    public NPCDialogue AfterKey;

    [Header("Exact Indices from catLeoDialogue Screenshot")]
    public int keyQuestStartIndex = 5;      // Dialogue line index for starting the key quest
    public int fireQuestStartIndex = 6;     // Dialogue line index for starting the firewood quest
    public int quizQuestionIndex = 7;       // Dialogue line index of the quiz question
    public int quizCorrectAnswerIndex = 8;  // Dialogue line index for a correct quiz answer
    public int quizWrongAnswerIndex = 9;    // Dialogue line index for a wrong quiz answer

    private bool keyQuestActive = false;
    private bool fireQuestActive = false;
    private bool quizActive = false;
    private bool quizSolved = false;

    [Header("Condition")]
    public int mouseCount = 1;
    public int keyCount = 1;
    public int WoodCount = 10;

    [Header("UI Elements (Panel 2)")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;

    [Header("Choice Buttons (Dynamic)")]
    public Button choiceButtonPrefab;
    public Transform choiceContainer;

    private readonly List<GameObject> spawnedButtons = new List<GameObject>();

    private int dialogueIndex;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private bool isWaitingForChoice = false;

    private string[] activeLines;
    private bool hasStartedQuest = false;

    void Start()
    {
        // Ensures fixed dialogue indices are not overwritten by outdated Inspector values
        keyQuestStartIndex = 5;
        fireQuestStartIndex = 6;
        quizQuestionIndex = 7;
        quizCorrectAnswerIndex = 8;
        quizWrongAnswerIndex = 9;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        HideAllChoices();
    }

    public void Interact()
    {
        if (dialogueData == null) return;

        if (!isDialogueActive)
        {
            StartDialogue();
        }
        else if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = activeLines[dialogueIndex];
            isTyping = false;
            CheckAndShowChoices();
        }
        else if (!isWaitingForChoice)
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
        if (SceneData.isQuizFireWoodKeyCompleted && AfterKey != null)
        {
            dialogueData = AfterKey;
            dialogueIndex = 0;
        }
        // If the wood quest is active and enough wood has been collected -> show AfterKey (hint)
        else if (fireQuestActive && (TaskCollectibleState.WoodCount >= WoodCount || SceneData.savedWoodCount >= WoodCount) && AfterKey != null)
        {
            dialogueData = AfterKey;
            dialogueIndex = 0;
        }
        // 1. If the key has been found -> AfterKey dialogue
        else if (keyQuestActive && (TaskCollectibleState.KeyCount >= keyCount || SceneData.savedKeyCount >= keyCount) && AfterKey != null)
        {
            dialogueData = AfterKey;
            dialogueIndex = 0;
        }
        // 2. If key quest or wood quest is active, but items are still missing -> KeepGoingDialogue
        else if ((keyQuestActive || fireQuestActive) && KeepGoingDialogue != null)
        {
            dialogueData = KeepGoingDialogue;
            dialogueIndex = 0;
        }
        // 3. If the mouse has been caught -> AfterMouse (catLeoDialogue)
        else if ((TaskCollectibleState.MouseCount >= mouseCount || SceneData.savedMouseCount >= mouseCount) && AfterMouse != null)
        {
            dialogueData = AfterMouse;
            hasStartedQuest = true;

            // If the quiz has already been answered correctly, directly show element 8 ("Correct!...")
            if (quizSolved)
            {
                dialogueIndex = quizCorrectAnswerIndex;
            }
            // If the player was in the quiz (and answered wrong or exited), restart directly at element 7 (quiz question)
            else if (quizActive)
            {
                dialogueIndex = quizQuestionIndex;
            }
            else
            {
                dialogueIndex = 0;
            }
        }
        else
        {
            dialogueIndex = 0;
        }

        isDialogueActive = true;
        isWaitingForChoice = false;

        if (dialogueData == null)
        {
            Debug.LogError("Kein DialogueData zugewiesen oder gefunden!");
            isDialogueActive = false;
            return;
        }

        if (nameText != null) nameText.text = dialogueData.npcName;
        if (portraitImage != null && dialogueData.npcPortrait != null)
            portraitImage.sprite = dialogueData.npcPortrait;

        LoadLanguageLines();

        if (activeLines == null || activeLines.Length == 0)
        {
            isDialogueActive = false;
            return;
        }

        HideAllChoices();
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());
    }

    private void LoadLanguageLines()
    {
        int lang = PlayerPrefs.GetInt("Language", 0);

        if (lang == 0) activeLines = dialogueData.dialogueLines;
        else if (lang == 1) activeLines = dialogueData.germanLines;
        else if (lang == 2) activeLines = dialogueData.spanishLines;

        if (activeLines == null || activeLines.Length == 0)
        {
            activeLines = dialogueData.dialogueLines;
        }
    }

    private void NextLine()
    {
        if (dialogueData == AfterMouse)
        {
            // When "Wrong / Falsch." (element 9) is displayed and the player presses E:
            // Immediately jump back to the quiz question (element 7) including answer buttons
            if (dialogueIndex == quizWrongAnswerIndex)
            {
                dialogueIndex = quizQuestionIndex;
                StartCoroutine(TypeLine());
                return;
            }

            // When element 5 (bird/key), element 6 (wood), or element 8 (correct answer) has been read:
            // Cleanly close the dialogue so it does NOT continue into the next line
            if (dialogueIndex == keyQuestStartIndex ||
                dialogueIndex == fireQuestStartIndex ||
                dialogueIndex == quizCorrectAnswerIndex)
            {
                EndDialogue();
                return;
            }
        }

        // Standard check for EndDialogueLines
        if (dialogueData.endDialogueLines != null &&
            dialogueIndex < dialogueData.endDialogueLines.Length &&
            dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

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
        CheckAndShowChoices();
    }

    private void CheckAndShowChoices()
    {
        if (dialogueData.choices == null) return;

        foreach (DialogueChoice choice in dialogueData.choices)
        {
            if (choice.dialogueIndex != dialogueIndex) continue;

            isWaitingForChoice = true;
            HideAllChoices();

            int lang = PlayerPrefs.GetInt("Language", 0);
            string[] activeChoices = choice.choices;
            if (lang == 1) activeChoices = choice.germanChoices;
            else if (lang == 2) activeChoices = choice.spanishChoices;
            if (activeChoices == null || activeChoices.Length == 0)
                activeChoices = choice.choices;

            for (int i = 0; i < activeChoices.Length; i++)
            {
                Button btn = Instantiate(choiceButtonPrefab, choiceContainer);
                spawnedButtons.Add(btn.gameObject);

                TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
                if (label != null) label.text = activeChoices[i];

                int nextIndex = (choice.nextDialogueIndexes != null && i < choice.nextDialogueIndexes.Length)
                    ? choice.nextDialogueIndexes[i] : -1;

                btn.onClick.AddListener(() =>
                {
                    MakeChoice(nextIndex);
                });
            }
            return;
        }
    }

    public void MakeChoice(int nextIndex)
    {
        isWaitingForChoice = false;
        HideAllChoices();

        // Activate the corresponding quest or quiz state depending on the selected dialogue path
        if (dialogueData == AfterMouse)
        {
            if (nextIndex == keyQuestStartIndex)
            {
                keyQuestActive = true;
            }
            else if (nextIndex == fireQuestStartIndex)
            {
                fireQuestActive = true;
            }
            else if (nextIndex == quizQuestionIndex)
            {
                quizActive = true;
            }
            else if (nextIndex == quizCorrectAnswerIndex)
            {
                quizSolved = true;
                Task5Quiz quizTask = GetComponent<Task5Quiz>();
                if (quizTask != null) quizTask.playerWonQuiz = true;
            }
            else if (nextIndex == quizWrongAnswerIndex)
            {
                quizActive = true;
            }
        }

        if (activeLines == null || nextIndex < 0 || nextIndex >= activeLines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueIndex = nextIndex;
        StartCoroutine(TypeLine());
    }

    private void HideAllChoices()
    {
        foreach (GameObject go in spawnedButtons)
        {
            if (go != null) Destroy(go);
        }
        spawnedButtons.Clear();
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        isTyping = false;
        isWaitingForChoice = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        HideAllChoices();
    }

    public bool IsTalking()
    {
        return isDialogueActive;
    }

    public int GetCurrentIndex()
    {
        return dialogueIndex;
    }
}