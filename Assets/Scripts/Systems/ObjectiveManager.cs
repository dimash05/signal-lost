using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [SerializeField] private List<Terminal> terminals = new List<Terminal>();
    [SerializeField] private FinalConsole finalConsole;

    public IReadOnlyList<Terminal> Terminals => terminals;
    public bool AllTerminalsActive =>
        terminals != null && terminals.Count > 0 && terminals.All(t => t && t.IsActivated);

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(this); return; }
        Instance = this;

        if (terminals == null || terminals.Count == 0)
            terminals = FindObjectsOfType<Terminal>(true).ToList();

        if (!finalConsole)
            finalConsole = FindObjectOfType<FinalConsole>(true);

        UpdateState();
    }

    public void NotifyTerminalActivated(Terminal t) => UpdateState();

    private void UpdateState()
    {
        int done  = terminals.Count(t => t && t.IsActivated);
        int total = terminals.Count(t => t);

        UIController.Instance?.SetObjectiveText($"Activate terminals: {done}/{total}");

        if (finalConsole)
            finalConsole.SetLocked(!AllTerminalsActive);
    }
}
