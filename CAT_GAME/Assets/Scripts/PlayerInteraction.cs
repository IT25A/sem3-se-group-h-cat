using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
	[Header("Einstellungen")]
	public float interactionRadius = 1.0f;
	public LayerMask interactableLayer; // Aktuell nicht im Filter genutzt, aber bereit für später

	// Wird vom Input System gesendet
	void OnInteract(InputValue value)
	{
		if (value.isPressed)
		{
			Debug.Log("E-Taste wurde gedrückt");

			Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

			if (hitObjects.Length > 0)
			{
				NPCTalk targetNpc = null;

				// Wir durchsuchen alle getroffenen Collider nach dem NPC-Skript
				foreach (var hit in hitObjects)
				{
					if (hit.gameObject != this.gameObject) // Ignoriere den Spieler
					{
						IInteractable target = hit.GetComponent<IInteractable>();
						if (target != null)
						{
							target.Interact();
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