// Schnittstelle für alle Objekte, mit denen der Spieler per Interaktionstaste interagieren kann
public interface IInteractable
{
    public void Interact();
    public bool CanInteract();
}