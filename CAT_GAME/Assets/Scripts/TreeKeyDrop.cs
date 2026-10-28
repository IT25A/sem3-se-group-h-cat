using UnityEngine;

public class TreeKeyDrop : MonoBehaviour, IInteractable
{
    public GameObject keyObject;
    public Transform landingPoint;
    public float fallSpeed = 3f;
    private bool hasDropped = false;
    private bool isFalling = false;

    public bool CanInteract()
    {
        return !hasDropped;
        
    }

    public void Interact()
    {
        if (!CanInteract() || hasDropped)
        {
            return;
        }
        CanInteract();
        hasDropped = true;
        isFalling = true;
        keyObject.SetActive(true);
    }

    void Update()
    {
        if (isFalling && keyObject != null)
        {
            keyObject.transform.position = Vector3.MoveTowards(keyObject.transform.position, landingPoint.position, fallSpeed * Time.deltaTime);
        }
    }
}
