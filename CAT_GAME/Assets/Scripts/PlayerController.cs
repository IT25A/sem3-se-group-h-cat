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
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);

            if (hitObjects.Length > 0)
            {
                GameObject ziel = hitObjects[0].gameObject;
                Debug.Log("Schritt 1: Simba berührt " + ziel.name);

                NPCTalk npc = ziel.GetComponent<NPCTalk>();
                
                if (npc != null)
                {
                    Debug.Log("Schritt 2: NPCTalk-Skript gefunden! Öffne Fenster...");
                    
                    if (npc.isDialogueActive)
                        npc.NextLine();
                    else
                        npc.StartDialogue();
                }
                else
                {
                    Debug.LogError("Fehler: Auf " + ziel.name + " wurde kein NPCTalk-Skript gefunden!");
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