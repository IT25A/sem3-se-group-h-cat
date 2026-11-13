using System.Collections.Generic;
using UnityEngine;

public class ShadowFollower : MonoBehaviour, IInteractable
{
    [Header("Referenzen")]
    public Transform playerTarget;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isFollowing = false;
    public float stopDistance = 0.5f;

    [Header("Verzögerung")]
    [Tooltip("Wie viele Positionen soll der NPC abwarten, bevor er losläuft?")]
    public int delayFrames = 25; 
    public float moveSpeed = 5f;

    [Header("Doctor Stay Settings")]
    public Transform doctorTarget;
    [Tooltip("Positionsversatz relativ zum Arzt, wenn der Hund dort abgesetzt wird (z. B. -1.2 für links, +1.2 für rechts)")]
    public Vector2 doctorOffset = new Vector2(-1.2f, 0f);
    public bool hasStayedAtDoctor = false;

    // Puffer für vergangene Spielerpositionen, um dem Spieler zeitverzögert auf demselben Pfad zu folgen
    private Queue<Vector2> positionHistory = new Queue<Vector2>();

    void Start()
    {
        NPCTalk npcTalk = GetComponent<NPCTalk>();
        if (npcTalk != null)
        {
            npcTalk.onDialogueEnded.AddListener(StartFollowing);
        }

        // Arzt-NPC automatisch in der Szene suchen, falls er nicht im Inspector zugewiesen wurde
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

        // Physische Kollision zwischen Spieler und Begleiter deaktivieren, damit der Begleiter den Spieler nicht blockiert
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

    // Platziert den Begleiter dauerhaft neben dem Arzt und beendet die Verfolgung
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

        positionHistory.Enqueue(playerTarget.position);
            
        // Erst loslaufen, sobald genügend Positionen im Verzögerungspuffer gespeichert sind
        if (positionHistory.Count > delayFrames )
        {
            Vector2 targetPosition = positionHistory.Dequeue();
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
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
    }

    private void UpdateAnimation(Vector2 direction)
    {
        if (direction.x < -0.01f) spriteRenderer.flipX = true;
        else if (direction.x > 0.01f) spriteRenderer.flipX = false;

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