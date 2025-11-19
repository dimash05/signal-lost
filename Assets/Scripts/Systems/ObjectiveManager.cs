using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [Header("Goals")]
    [SerializeField] private int totalTerminals = 3;
    [SerializeField] private DoorAnimator[] doorsToOpen;
    [SerializeField] private GameObject finalConsole;

    private int activated;

    public int TotalTerminals => totalTerminals;
    public int Activated      => activated;
    public bool Completed     => activated >= totalTerminals;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (finalConsole) finalConsole.SetActive(false);
        UpdateUI();
    }

    public void OnTerminalActivated()
    {
        activated = Mathf.Clamp(activated + 1, 0, totalTerminals);
        UpdateUI();

        if (Completed)
        {
            if (doorsToOpen != null)
                foreach (var d in doorsToOpen) if (d) d.Open();

            if (finalConsole) finalConsole.SetActive(true);

            UIController.Instance?.SetObjective("Get to antena and send the signal");
        }
    }

    private void UpdateUI()
    {
        if (!Completed)
            UIController.Instance?.SetObjective($"Activate terminals: {activated}/{totalTerminals}");
    }

    public void ResetProgress()
    {
        activated = 0;
        UpdateUI();
        if (finalConsole) finalConsole.SetActive(false);
        if (doorsToOpen != null)
            foreach (var d in doorsToOpen) if (d) d.Close();
    }
}
