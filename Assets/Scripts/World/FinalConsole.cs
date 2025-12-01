using UnityEngine;

public class FinalConsole : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptReady  = "Send SOS (E)";
    [SerializeField] private string promptLocked = "Activate all terminals";

    private bool locked = true;
    public string Prompt => locked ? promptLocked : promptReady;

    public void SetLocked(bool value) => locked = value;

    public void Interact(PlayerInteractor interactor)
    {
        if (locked)
        {
            ToastUI.ShowOnce("final_locked", "You must activate all terminals first.", 2.5f);
            return;
        }

        UIController.Instance?.HidePrompt();
        UIController.Instance?.ShowWinScreen();
        ToastUI.Show("SOS sent. Extraction inbound.", 2.5f);
    }
}
