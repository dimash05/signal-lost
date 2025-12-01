using UnityEngine;

public class GameStartHints : MonoBehaviour
{
    [SerializeField] private bool showOnStart = true;

    void Start()
    {
        if (!showOnStart) return;

        ToastUI.ShowOnce("tip_flashlight", "Press F to toggle the flashlight.", 2.2f, true);
        ToastUI.ShowOnce("tip_goal", "Activate 3 terminals, then send SOS at the final console.", 3.0f, true);
    }
}
