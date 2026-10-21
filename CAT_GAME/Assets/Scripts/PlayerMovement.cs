using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[Header("Einstellungen")]
	public float moveSpeed = 5f;

	[Header("Referenzen")]
	public Rigidbody2D rb;

	private Vector2 movement;

	void Start()
	{
		if (rb == null) rb = GetComponent<Rigidbody2D>();
	}

	// Wird vom Input System gesendet
	void OnMove(InputValue value)
	{
		movement = value.Get<Vector2>();
	}

	private void FixedUpdate()
	{
		if (rb != null)
		{
			// velocity überlässt der Physik-Engine die volle Kontrolle
			rb.linearVelocity = movement * moveSpeed;
		}
	}
}