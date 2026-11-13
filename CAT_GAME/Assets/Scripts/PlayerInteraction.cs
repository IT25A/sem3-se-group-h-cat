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

            // Priorität 1: Wenn bereits ein Dialog läuft, schalte diesen weiter
            if (NPCTalk.activeTalker != null && NPCTalk.activeTalker.isDialogueActive)
            {
                NPCTalk.activeTalker.Interact();
                return;
            }

            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, interactionRadius);

            // Das erste gültige interagierbare Objekt im Umkreis (außer dem Spieler selbst) auslösen
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