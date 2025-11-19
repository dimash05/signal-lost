using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] DoorAnimator door;
    [SerializeField] bool requireAllTerminals = true;

    bool Unlocked =>
        !requireAllTerminals ||
        (ObjectiveManager.Instance != null && ObjectiveManager.Instance.Completed);

    void Reset() => door = GetComponent<DoorAnimator>();

    public string GetPrompt()
    {
        if (!Unlocked) return "Door is closed. Activate all terminals";
        return door != null && door.IsOpen ? "E — Close the door" : "E — Open the door";
    }

    public void Interact()
    {
        if (!Unlocked || door == null) return;
        door.Toggle();
    }
}
