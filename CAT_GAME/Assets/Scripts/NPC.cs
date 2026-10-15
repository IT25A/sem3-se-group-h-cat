using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    // Array für beliebig viele Wegpunkte
    public Transform[] waypoints;
    private int currentPointIndex = 0;

    private void FixedUpdate()
    {
        // Abbruch, falls keine Punkte im Inspector zugewiesen wurden
        if (waypoints.Length == 0) return;

        // 1. Richtung zum aktuellen Zielpunkt berechnen
        Transform target = waypoints[currentPointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        // 2. NPC bewegen (verhindert das Durchrutschen durch Wände)
        rb.linearVelocity = direction * moveSpeed;

        // 3. Ausrichtung der Katze (links/rechts)
        if (direction.x < 0) spriteRenderer.flipX = true;
        else if (direction.x > 0) spriteRenderer.flipX = false;

        animator.SetBool("isWalking", true);

        // 4. Prüfen, ob der Punkt erreicht wurde (auf 0.2 Units genau)
        if (Vector2.Distance(transform.position, target.position) < 1f)
        {
            currentPointIndex++; // Nächster Punkt
            if (currentPointIndex >= waypoints.Length)
            {
                currentPointIndex = 0; // Wieder von vorne anfangen
            }
        }
    }
}