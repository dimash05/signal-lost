using UnityEngine;

public class Terminal : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer screen;     
    [SerializeField] private Color offColor = Color.black;
    [SerializeField] private Color onColor  = Color.green;
    [SerializeField] private AudioSource au;
    [SerializeField] private AudioClip activateBeep;

    private bool active;

    void Start()
    {
        if (screen && screen.material.HasProperty("_EmissionColor"))
        {
            var m = screen.material;
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", offColor);
        }
    }

    public string GetPrompt() => active ? "Terminal activated" : "Activate terminal";

    public void Interact()
    {
        if (active) return;
        active = true;

        if (screen && screen.material.HasProperty("_EmissionColor"))
        {
            var m = screen.material;
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", onColor);
        }

        if (au && activateBeep) au.PlayOneShot(activateBeep, 0.85f);
        ObjectiveManager.Instance?.OnTerminalActivated();
    }
}
