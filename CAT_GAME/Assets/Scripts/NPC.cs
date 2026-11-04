using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public bool isInteracting = false;

    // Array für beliebig viele Wegpunkte
    public Transform[] waypoints;
    private int currentPointIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isInteracting)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false); // Wichtig: Animation stoppen beim Reden
            return;
        }

        // Abbruch, falls keine Punkte im Inspector zugewiesen wurden
        if (waypoints.Length == 0) return;

        // 1. Richtung zum aktuellen Zielpunkt berechnen
        Transform target = waypoints[currentPointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        // 2. NPC bewegen
        rb.linearVelocity = direction * moveSpeed;

        // 3. Ausrichtung der Katze (Links/Rechts spiegeln bleibt)
        if (direction.x < 0) spriteRenderer.flipX = true;
        else if (direction.x > 0) spriteRenderer.flipX = false;

        // 4. Richtungswerte an den Animator senden
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
        animator.SetBool("isWalking", true);

        // 5. Prüfen, ob der Punkt erreicht wurde
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentPointIndex++; // Nächster Punkt
            if (currentPointIndex >= waypoints.Length)
            {
                currentPointIndex = 0; // Wieder von vorne anfangen
            }
        }
    }
}