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

    bool _wasAllActive = false;

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(this); return; }
        Instance = this;

        if (terminals == null || terminals.Count == 0)
            terminals = FindObjectsOfType<Terminal>(true).ToList();

        if (!finalConsole)
            finalConsole = FindObjectOfType<FinalConsole>(true);

        UpdateState(initial:true);
    }

    public void NotifyTerminalActivated(Terminal t)
    {
        UpdateState(initial:false);
    }

    private void UpdateState(bool initial)
    {
        int done  = terminals.Count(t => t && t.IsActivated);
        int total = terminals.Count(t => t);
        UIController.Instance?.SetObjectiveText($"Activate terminals: {done}/{total}");

        bool allNow = AllTerminalsActive;

        if (finalConsole)
            finalConsole.SetLocked(!allNow);

        if (!initial)
        {
            ToastUI.Show($"Terminal activated ({done}/{total}).", 2.0f);
        }

        if (!_wasAllActive && allNow)
        {
            ToastUI.Show("All terminals are active. Send SOS at the final console.", 3.0f);
        }

        _wasAllActive = allNow;
    }
}
