using System.Collections.Generic;
using UnityEngine;

public class ShadowFollower : MonoBehaviour
{
    [Header("Referenzen")]
    public Transform playerTarget; // Ziehe deinen Spieler hier rein
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public bool isFollowing = false;
    public float stopDistance = 1.2f;
    public void StartFollowing() => isFollowing = true;

    [Header("Verzögerung")]
    [Tooltip("Wie viele Positionen soll der NPC abwarten, bevor er losläuft?")]
    public int delayFrames = 25; 
    public float moveSpeed = 2f; // Sollte exakt der Speed deines Spielers sein

    // Die Liste, die die alten Positionen des Spielers speichert
    private Queue<Vector2> positionHistory = new Queue<Vector2>();

    void Start()
    {
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

    void FixedUpdate()
    {
        if (playerTarget == null) return;

        // 1. Speichere die aktuelle Position des Spielers ab
        positionHistory.Enqueue(playerTarget.position);

        // 2. Warte, bis der Puffer voll ist (Verzögerung)
        if (positionHistory.Count > delayFrames)
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