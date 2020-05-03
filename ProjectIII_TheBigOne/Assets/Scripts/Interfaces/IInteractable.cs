public interface IInteractable
{
    bool Interact();

    void OnStartInteract();
    void OnInteracting();
    void OnEndInteract();
}
