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
        // Sorgt für eigentliche Bewegung des physikalischen Körpers
        rigidbody.MovePosition(rigidbody.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);

            if (hitObjects.Length > 0)
            {
                Debug.Log("Simba redet gerade mit: " + hitObjects[0].gameObject.name)
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}