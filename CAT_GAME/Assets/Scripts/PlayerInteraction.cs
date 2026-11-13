using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float interactionRadius = 1.0f;
    public LayerMask interactableLayer; 

    // Sent by the Input System
    void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("E-Taste wurde gedrückt");

            // Priority 1: If a dialogue is already active, advance it
            if (NPCTalk.activeTalker != null && NPCTalk.activeTalker.isDialogueActive)
            {
                NPCTalk.activeTalker.Interact();
                return;
            }

            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

            // Trigger the first valid interactable object within range (excluding the player itself)
            if (hitObjects.Length > 0)
            {
                bool interacted = false;

                foreach (var hit in hitObjects)
                {
                    if (hit.gameObject != this.gameObject)
                    {
                        IInteractable target = hit.GetComponent<IInteractable>();
                        if (target != null && target.CanInteract())
                        {
                            target.Interact();
                            interacted = true;
                            Debug.Log("DIAGNOSE: Interaktion mit " + hit.gameObject.name + " erfolgreich ausgelöst!");
                            break;
                        }
                    }
                }

                if (!interacted)
                {
                    Debug.LogWarning("DIAGNOSE: Kein interagierbares Objekt im Radius gefunden.");
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