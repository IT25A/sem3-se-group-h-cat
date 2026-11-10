using System.Collections.Generic;
using UnityEngine;

public class ShadowFollower : MonoBehaviour, IInteractable
{
    [Header("Referenzen")]
    public Transform playerTarget; // Ziehe deinen Spieler hier rein
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isFollowing = false;
    public float stopDistance = 0.5f;

    [Header("Verzögerung")]
    [Tooltip("Wie viele Positionen soll der NPC abwarten, bevor er losläuft?")]
    public int delayFrames = 25; 
    public float moveSpeed = 5f; // Sollte exakt der Speed deines Spielers sein

    [Header("Doctor Stay Settings")]
    public Transform doctorTarget;
    [Tooltip("Offset relative to the doctor when placing the dog (e.g. -1.2 for left, +1.2 for right)")]
    public Vector2 doctorOffset = new Vector2(-1.2f, 0f);
    public bool hasStayedAtDoctor = false;

    // Die Liste, die die alten Positionen des Spielers speichert
    private Queue<Vector2> positionHistory = new Queue<Vector2>();

    void Start()
    {
        NPCTalk npcTalk = GetComponent<NPCTalk>();
        if (npcTalk != null)
        {
            npcTalk.onDialogueEnded.AddListener(StartFollowing);
        }

        if (doctorTarget == null)
        {
            NPCTalk[] allNpcs = FindObjectsByType<NPCTalk>(FindObjectsSortMode.None);
            foreach (NPCTalk talker in allNpcs)
            {
                if (talker.gameObject.name == "Doctor" || (talker.dialogueData != null && talker.dialogueData.npcName != null && talker.dialogueData.npcName.Contains("Doctor")))
                {
                    doctorTarget = talker.transform;
                    break;
                }
            }
        }

        if (doctorTarget != null)
        {
            NPCTalk doctorTalk = doctorTarget.GetComponent<NPCTalk>();
            if (doctorTalk != null)
            {
                doctorTalk.onDialogueEnded.AddListener(StayAtDoctorPosition);
            }
        }

        if (rb == null) rb = GetComponent<Rigidbody2D>();

        Collider2D myCollider = GetComponent<Collider2D>();
        if (myCollider != null && playerTarget != null)
        {
            Collider2D playerCollider = playerTarget.GetComponent<Collider2D>();

            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, playerCollider, true);
            }
        }
    }
    public void StartFollowing()
    {
        isFollowing = true;
    }

    public void Interact()
    {
        if (CanInteract())
        {
            StartFollowing();
        }
    }

    public bool CanInteract()
    {
        return !isFollowing && !hasStayedAtDoctor;
    }

    public void StayAtDoctorPosition()
    {
        if (hasStayedAtDoctor || !isFollowing) return;

        hasStayedAtDoctor = true;
        isFollowing = false;
        positionHistory.Clear();

        if (doctorTarget != null)
        {
            transform.position = (Vector2)doctorTarget.position + doctorOffset;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = doctorOffset.x >= 0;
        }

        this.enabled = false;
    }


    void FixedUpdate()
    {
        if (playerTarget == null || !isFollowing) return;

        // 1. Speichere die aktuelle Position des Spielers ab
        positionHistory.Enqueue(playerTarget.position);
            
         // 2. Warte, bis der Puffer voll ist (Verzögerung)
        if (positionHistory.Count > delayFrames )
        {
            // 3. Hole die älteste gespeicherte Position aus der Liste
            Vector2 targetPosition = positionHistory.Dequeue();

            // 4. Bewege den NPC zu dieser Position
            Vector2 currentPos = transform.position;
            float distance = Vector2.Distance(currentPos, playerTarget.position);

            if (distance > stopDistance)
            {
                Vector2 direction = (targetPosition - currentPos).normalized;
                rb.linearVelocity = direction * moveSpeed;
                UpdateAnimation(direction);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            // Puffer füllt sich noch, NPC bleibt stehen
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
    }

    private void UpdateAnimation(Vector2 direction)
    {
        // Links/Rechts drehen
        if (direction.x < -0.01f) spriteRenderer.flipX = true;
        else if (direction.x > 0.01f) spriteRenderer.flipX = false;

        // Richtung an Animator senden
        if (direction.magnitude > 0.01f)
        {
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}