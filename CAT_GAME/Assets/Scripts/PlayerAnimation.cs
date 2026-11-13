using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	[Header("Referenzen")]
	public Animator animator;
	public SpriteRenderer spriteRenderer;
	public Rigidbody2D rb;

	void Start()
	{
		if (animator == null) animator = GetComponent<Animator>();
		if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
		if (rb == null) rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (rb == null) return;

		Vector2 velocity = rb.linearVelocity;

		// Sprite je nach horizontaler Bewegungsrichtung spiegeln und Laufanimation steuern
		if (velocity.x < -0.01f)
		{
			spriteRenderer.flipX = true;
		}
		else if (velocity.x > 0.01f)
		{
			spriteRenderer.flipX = false;
		}

		bool isMoving = velocity.sqrMagnitude > 0.01f;
		animator.SetBool("isWalking", isMoving);
	}
}