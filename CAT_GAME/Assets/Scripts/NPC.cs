using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public bool isInteracting = false;

    // Array for any number of waypoints
    public Transform[] waypoints;
    private int currentPointIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Pause movement and walking animation during dialogue
        if (isInteracting)
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }

        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentPointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;

        if (direction.x < 0) spriteRenderer.flipX = true;
        else if (direction.x > 0) spriteRenderer.flipX = false;

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
        animator.SetBool("isWalking", true);

        // Cycle to the next waypoint upon reaching the current one
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex >= waypoints.Length)
            {
                currentPointIndex = 0;
            }
        }
    }
}