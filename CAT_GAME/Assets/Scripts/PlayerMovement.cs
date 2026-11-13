using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[Header("Settings")]
	public float moveSpeed = 5f;

	[Header("References")]
	public Rigidbody2D rb;

	public bool canControl = true; // Flag, um die Kontrolle zu sperren

	public Transform autoWalkTarget; // Zielposition für das automatische Gehen

	public float stopDistance = 1.5f; // Abstand, bei dem das automatische Gehen stoppt

	[Header("Events")]
	public UnityEngine.Events.UnityEvent onReachedTarget;
	private bool hasReachedTarget = false;

	private Vector2 movement;

	
	void Start()
	{
		if (rb == null) rb = GetComponent<Rigidbody2D>();
	}

	// Wird vom Input System gesendet
	void OnMove(InputValue value)
	{
		if (!canControl)
		{
			movement = Vector2.zero;
			return;
		}

		movement = value.Get<Vector2>();
	}

	private void FixedUpdate()
	{
		if (rb == null) return;

		if (canControl)
		{
			hasReachedTarget = false;
			rb.linearVelocity = movement * moveSpeed;
		}
		// Wenn die manuelle Steuerung deaktiviert ist, bewegt sich der Spieler automatisch zum Zielpunkt (z. B. für Zwischensequenzen)
		else if (autoWalkTarget != null)
		{
			float xDistance = autoWalkTarget.position.x - transform.position.x;

			if (xDistance > stopDistance)
			{
				rb.linearVelocity = new Vector2(moveSpeed, 0f);
			}
			else
			{
				rb.linearVelocity = Vector2.zero;
				if (!hasReachedTarget)
				{
					hasReachedTarget = true;
					onReachedTarget?.Invoke();
				}
			}
		}
	}
}