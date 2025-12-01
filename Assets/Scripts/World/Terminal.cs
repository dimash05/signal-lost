using UnityEngine;

public class Terminal : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt = "Activate terminal (E)";
    [SerializeField] private bool startActivated = false;

    public bool IsActivated { get; private set; }
    public string Prompt => IsActivated ? string.Empty : prompt;

    private void Awake()
    {
        IsActivated = startActivated;
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (IsActivated) return;

        IsActivated = true;
        UIController.Instance?.HidePrompt();
        ObjectiveManager.Instance?.NotifyTerminalActivated(this);
    }
}
