using UnityEngine;
using System;

public class TerminalState : MonoBehaviour
{
    public bool Activated { get; private set; }
    public static event Action<int,int> OnAnyTerminalProgress; 

    static int total;
    static int done;

    void Awake()
    {
        total++;
    }

    public void MarkActivated()
    {
        if (Activated) return;
        Activated = true;
        done = Mathf.Clamp(done + 1, 0, total);
        OnAnyTerminalProgress?.Invoke(done, total);
    }

    public static bool AllActivated => done >= total;
    public static int Total => total;
    public static int Done  => done;
}
