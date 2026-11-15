using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "NPCDialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;

    [Header("Dialog")]
    public string[] dialogueLines;//englische Version
    public string[] germanLines;
    public string[] spanishLines;
    public bool [] autoProgressLines;
    public float autoProgressDelay = 1.5f;
    public bool [] endDialogueLines;
    public float typingSpeed = 0.05f;
    public AudioClip[] voiceSound;
    public float voicePitch = 1.0f;

    public DialogueChoice [] choices;
}

[System.Serializable]

public class DialogueChoice
{
    public int dialogueIndex; //Dial choice where choices appear
    [Header("Button Texte")]
    public string []choices;//Player response options
    public string []germanChoices;
    public string []spanishChoices;
    public int [] nextDialogueIndexes; //Where choices lead
}
