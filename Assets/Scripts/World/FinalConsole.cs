using UnityEngine;

public class FinalConsole : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptReady  = "Send signal SOS (E)";
    [SerializeField] private string promptLocked = "Activate all terminals";
    private bool locked = true;

    public string Prompt => locked ? promptLocked : promptReady;

    public void SetLocked(bool value)
    {
        locked = value;
    }

    public void Interact(PlayerInteractor interactor)
    {
        if (locked) return;

        UIController.Instance?.HidePrompt();
        UIController.Instance?.ShowWinScreen();
    }
}
