using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    const string OPEN = "Open";
    public bool IsOpen { get; private set; }

    void Reset() => animator = GetComponent<Animator>();

    public void Open()  { if (!animator) animator = GetComponent<Animator>(); animator.SetBool(OPEN, true);  IsOpen = true; }
    public void Close() { if (!animator) animator = GetComponent<Animator>(); animator.SetBool(OPEN, false); IsOpen = false; }
    public void Toggle(){ if (IsOpen) Close(); else Open(); }
}
