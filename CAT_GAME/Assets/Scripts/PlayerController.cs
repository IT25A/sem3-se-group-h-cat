using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rigidbody;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float interactionRadius = 1.0f;
    public LayerMask interactableLayer;

    // speichert X- und Y-Richtung 
    private Vector2 movement;

    void OnMove(InputValue value)
    {
        // liest X- und Y-Wert direkt als 2D-Vektor aus
        movement = value.Get<Vector2>();
    }

    void Update()
    {
        // Ausrichtung der Katze
        if (movement.x < 0)
        {
            // Nach links 
            spriteRenderer.flipX = true;

        }
        else if (movement.x > 0)
        {
            // Nach rechts
            spriteRenderer.flipX = false;
        }
        bool isMoving = movement.x != 0 || movement.y != 0;
        animator.SetBool("isWalking", isMoving);
    }

    private void FixedUpdate()
    {
        // velocity überlässt der Physik-Engine die volle Kontrolle zur Kollisionsvermeidung
        rigidbody.linearVelocity = movement * moveSpeed;
    }

    void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("E-Taste wurde gedrückt");
            // Lösche den interactableLayer-Filter kurz testweise:
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

            if (hitObjects.Length > 0)
            {
                NPCTalk targetNpc = null;

                // Wir durchsuchen alle getroffenen Collider nach dem NPC-Skript (und überspringen den Spieler selbst)
                foreach (var hit in hitObjects)
                {
                    if (hit.gameObject != this.gameObject) // Ignoriere den Spieler
                    {
                        NPCTalk npc = hit.GetComponent<NPCTalk>();
                        if (npc != null)
                        {
                            targetNpc = npc;
                            break; // Gefunden! Schleife stoppen.
                        }
                    }
                }

                // Wenn ein NPC gefunden wurde, Dialog starten/fortsetzen
                if (targetNpc != null)
                {
                    Debug.Log("DIAGNOSE: NPC gefunden, starte Dialog!");
                    if (targetNpc.isDialogueActive)
                        targetNpc.NextLine();
                    else
                        targetNpc.StartDialogue();
                }
                else
                {
                    Debug.LogWarning("DIAGNOSE: Kein NPC mit NPCTalk-Skript im Radius gefunden.");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}