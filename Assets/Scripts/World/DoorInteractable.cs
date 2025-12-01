using UnityEngine;
using UnityEngine.Events;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("UI")]
    [SerializeField] private string prompt = "Open door (E)";

    [Header("Events")]
    public UnityEvent onInteract;

    public string Prompt => prompt;

    public void Interact(PlayerInteractor interactor)
    {
        onInteract?.Invoke();
    }
}
