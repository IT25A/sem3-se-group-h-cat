using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[Header("Settings")]
	public float moveSpeed = 5f;

	[Header("References")]
	public Rigidbody2D rb;

	public bool canControl = true; // Flag to lock player control

	public Transform autoWalkTarget; // Target position for automatic walking

	public float stopDistance = 1.5f; // Distance at which automatic walking stops

	[Header("Events")]
	public UnityEngine.Events.UnityEvent onReachedTarget;
	private bool hasReachedTarget = false;

	private Vector2 movement;

	
	void Start()
	{
		if (rb == null) rb = GetComponent<Rigidbody2D>();
	}

	// Sent by the Input System
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
		// When manual control is disabled, the player automatically walks toward the target point (e.g., for cutscenes)
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