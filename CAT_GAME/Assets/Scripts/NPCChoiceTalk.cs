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
    
    [Tooltip("Auf welchen Satz-Index (Next Dialogue Index) springt der Button 'Schlüssel finden'?")]
    public int keyQuestStartIndex = 3;
    private bool keyQuestActive = false; // Merkt sich, ob "Schlüssel finden" gewählt wurde

    [Header ("Condition")]
    public int mouseCount = 1;
    public int keyCount = 1;


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
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        HideAllChoices();
    }
    //e drücken stoppt die Animation des Typing
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
        // 1. Wenn die Schlüssel-Quest aktiv ist UND der Schlüssel gefunden wurde -> AfterKey
        if (keyQuestActive && (TaskCollectibleState.KeyCount >= keyCount || SceneData.savedKeyCount >= keyCount) && AfterKey != null)
        {
            dialogueData = AfterKey;
        }
        // 2. Wenn die Schlüssel-Quest aktiv ist, aber der Schlüssel noch FEHLT -> KeepGoingDialogue
        else if (keyQuestActive && KeepGoingDialogue != null)
        {
            dialogueData = KeepGoingDialogue;
        }
        // 3. Wenn die Maus gefangen wurde -> AfterMouse
        else if (SceneData.savedMouseCount >= mouseCount && AfterMouse != null)
        {
            dialogueData = AfterMouse;
            hasStartedQuest = true;
        }
        // 4. Fallback: Normaler Before-Mouse-Dialog bleibt unangetastet

        isDialogueActive = true;
        dialogueIndex = 0;
        isWaitingForChoice = false;

        // Sicherheitsprüfung
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
        // Wenn die aktuelle Zeile als Ende markiert ist, Dialog beenden
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
        Debug.Log("Tippen fertig bei Index " + dialogueIndex);

        CheckAndShowChoices();
    }

    private void CheckAndShowChoices()
    {
        Debug.Log("Check Choices bei Index " + dialogueIndex);
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
            Debug.Log("Choice gefunden, Anzahl: " + activeChoices.Length);

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
                    Debug.Log("Button geklickt, gehe zu Index " + nextIndex);
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

        // Wenn der Spieler im AfterMouse-Dialog auf den "Schlüssel finden"-Button klickt:
        if (dialogueData == AfterMouse && nextIndex == keyQuestStartIndex)
        {
            keyQuestActive = true;
            Debug.Log("Schlüssel-Quest wurde gestartet!");
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
}