using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("Battery")]
    [SerializeField] Slider  batterySlider;
    [SerializeField] TMP_Text batteryText;

    [Header("Oxygen")]
    [SerializeField] Slider  oxygenSlider;
    [SerializeField] TMP_Text oxygenText; 

    [Header("Texts")]
    [SerializeField] TMP_Text objectiveText;
    [SerializeField] TMP_Text promptText;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (batterySlider) { batterySlider.minValue = 0; batterySlider.maxValue = 1; batterySlider.value = 1f; }
        if (batteryText)   batteryText.text   = "100%";

        if (oxygenSlider)  { oxygenSlider.minValue = 0; oxygenSlider.maxValue = 1; oxygenSlider.value = 1f; }
        if (oxygenText)    oxygenText.text    = "100%";

        if (objectiveText) objectiveText.text = "";
        if (promptText)    promptText.text    = "";
    }

    public void SetBattery01(float t)
    {
        t = Mathf.Clamp01(t);
        if (batterySlider) batterySlider.value = t;
        if (batteryText)   batteryText.text    = Mathf.RoundToInt(t * 100f) + "%";
    }

    public void SetOxygen01(float t)
    {
        t = Mathf.Clamp01(t);
        if (oxygenSlider) oxygenSlider.value = t;
        if (oxygenText)   oxygenText.text    = Mathf.RoundToInt(t * 100f) + "%";   
       
    }

    public void SetObjective(string text) { if (objectiveText) objectiveText.text = text ?? ""; }
    public void SetPrompt(string text)    { if (promptText)    promptText.text    = text ?? ""; }
}
