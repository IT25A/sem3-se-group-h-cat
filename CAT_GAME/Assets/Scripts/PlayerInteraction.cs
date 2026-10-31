using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Einstellungen")]
    public float interactionRadius = 1.0f;
    public LayerMask interactableLayer; 

    // Wird vom Input System gesendet
    void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("E-Taste wurde gedrückt");

            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

            if (hitObjects.Length > 0)
            {
                bool interacted = false;

                // Wir durchsuchen alle getroffenen Collider nach einem IInteractable
                foreach (var hit in hitObjects)
                {
                    if (hit.gameObject != this.gameObject) // Ignoriere den Spieler
                    {
                        IInteractable target = hit.GetComponent<IInteractable>();
                        if (target != null)
                        {
                            target.Interact();
                            interacted = true;
                            Debug.Log("DIAGNOSE: Interaktion mit " + hit.gameObject.name + " erfolgreich ausgelöst!");
                            break; // Sobald ein Ziel gefunden wurde, abbrechen
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