using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class NPCChoiceTalk : MonoBehaviour, IInteractable
{
    [Header("Dialog Daten")]
    public NPCDialogue dialogueData;
    public NPCDialogue AfterMouse;
    public NPCDialogue KeepGoingDialogue;
    public NPCDialogue AfterKey;

    [Header("Exakte Indizes aus deinem catLeoDialogue Screenshot")]
    public int keyQuestStartIndex = 5;      // Element 5: "A bird stole it..."
    public int fireQuestStartIndex = 6;     // Element 6: "Could you gather 10 pieces of wood..."
    public int quizQuestionIndex = 7;       // Element 7: "Cats sleep a lot..."
    public int quizCorrectAnswerIndex = 8;  // Element 8: "Correct! Hmm..."
    public int quizWrongAnswerIndex = 9;    // Element 9: "Wrong / Falsch."

    private bool keyQuestActive = false;
    private bool fireQuestActive = false;
    private bool quizActive = false;
    private bool quizSolved = false;

    [Header("Condition")]
    public int mouseCount = 1;
    public int keyCount = 1;
    public int WoodCount = 10;

    [Header("UI Elemente (Panel 2)")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;

    [Header("Choice Buttons (dynamisch)")]
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
        // Überschreibt eventuelle alte Werte im Inspector automatisch mit deinen echten Zeilen aus dem Screenshot:
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

        // 1. Wenn Schlüssel gefunden -> AfterKey Dialog
        else if (keyQuestActive && (TaskCollectibleState.KeyCount >= keyCount || SceneData.savedKeyCount >= keyCount) && AfterKey != null)
        {
            dialogueData = AfterKey;
            dialogueIndex = 0;
        }
        // 2. Wenn Schlüssel-Quest aktiv, aber Schlüssel fehlt -> KeepGoingDialogue
        else if (keyQuestActive && KeepGoingDialogue != null)
        {
            dialogueData = KeepGoingDialogue;
            dialogueIndex = 0;
        }
        // 3. Wenn die Maus gefangen wurde -> AfterMouse (catLeoDialogue)
        else if ((TaskCollectibleState.MouseCount >= mouseCount || SceneData.savedMouseCount >= mouseCount) && AfterMouse != null)
        {
            dialogueData = AfterMouse;
            hasStartedQuest = true;

            // Falls das Quiz schon richtig beantwortet wurde, zeige direkt Element 8 ("Correct!...")
            if (quizSolved)
            {
                dialogueIndex = quizCorrectAnswerIndex;
            }
            // Falls der Spieler im Quiz war (und falsch lag oder rausgegangen ist), starte direkt wieder bei Element 7 (Quizfrage)!
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
            // Wenn "Wrong / Falsch." (Element 9) angezeigt wird und der Spieler E drückt:
            // Springe sofort wieder zurück zur Quizfrage (Element 7) inkl. Antwort-Buttons!
            if (dialogueIndex == quizWrongAnswerIndex)
            {
                dialogueIndex = quizQuestionIndex;
                StartCoroutine(TypeLine());
                return;
            }

            // Wenn Element 5 (Vogel/Schlüssel), Element 6 (Holz) oder Element 8 (Richtige Antwort) zu Ende gelesen wurde:
            // Dialog sauber schließen, damit er NICHT in den nächsten Satz weiterläuft!
            if (dialogueIndex == keyQuestStartIndex ||
                dialogueIndex == fireQuestStartIndex ||
                dialogueIndex == quizCorrectAnswerIndex)
            {
                EndDialogue();
                return;
            }
        }

        // Standard-Prüfung auf EndDialogueLines
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

        if (dialogueData == AfterMouse)
        {
            if (nextIndex == keyQuestStartIndex) // 5
            {
                keyQuestActive = true;
            }
            else if (nextIndex == fireQuestStartIndex) // 6
            {
                fireQuestActive = true;
            }
            else if (nextIndex == quizQuestionIndex) // 7
            {
                quizActive = true;
            }
            else if (nextIndex == quizCorrectAnswerIndex) // 8
            {
                quizSolved = true;
                Task5Quiz quizTask = GetComponent<Task5Quiz>();
                if (quizTask != null) quizTask.playerWonQuiz = true;
            }
            else if (nextIndex == quizWrongAnswerIndex) // 9
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