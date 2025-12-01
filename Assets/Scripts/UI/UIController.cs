using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [Header("HUD")]
    [SerializeField] private Slider batterySlider;
    [SerializeField] private TMP_Text batteryText;
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private TMP_Text oxygenText;
    [SerializeField] private TMP_Text objectiveText;

    [Header("Prompt / Tutorial")]
    [SerializeField] private CanvasGroup promptPanel;
    [SerializeField] private TMP_Text promptText;

    [Header("Screens")]
    [SerializeField] private CanvasGroup winScreen;   
    [SerializeField] private CanvasGroup deathScreen;  

    [Header("Optional Fade")]
    [SerializeField] private CanvasGroup fade;       

    [Header("Debug")]
    [SerializeField] private bool logUiEvents = false;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        HideGroup(winScreen);
        HideGroup(deathScreen);
        HideGroup(promptPanel);
    }

    #region --- Helpers ---
   private static void ShowGroup(CanvasGroup g)
{
    if (!g) return;
    if (!g.gameObject.activeSelf) g.gameObject.SetActive(true); 
    g.alpha = 1f;
    g.interactable = true;
    g.blocksRaycasts = true;
}

private static void HideGroup(CanvasGroup g)
{
    if (!g) return;
    g.alpha = 0f;
    g.interactable = false;
    g.blocksRaycasts = false;
    if (!g.gameObject.activeSelf) g.gameObject.SetActive(true);
}

    private void Log(string msg)
    {
        if (logUiEvents) Debug.Log(msg);
    }
    #endregion

    public void SetBattery(float value01)
    {
        value01 = Mathf.Clamp01(value01);
        if (batterySlider) batterySlider.value = value01;
        if (batteryText)   batteryText.text = Mathf.RoundToInt(value01 * 100f) + "%";
    }

    public void Battery01(float value01) => SetBattery(value01);
    public void SetBattery01(float value01) => SetBattery(value01);

    public void SetOxygen01(float value01)
    {
        value01 = Mathf.Clamp01(value01);
        if (oxygenSlider) oxygenSlider.value = value01;
        if (oxygenText)   oxygenText.text = Mathf.RoundToInt(value01 * 100f) + "%";
    }

    public void SetOxygenSeconds(float secondsLeft, float maxSeconds = 0f)
    {
        secondsLeft = Mathf.Max(0f, secondsLeft);
        if (oxygenText) oxygenText.text = Mathf.CeilToInt(secondsLeft) + "s";
        if (oxygenSlider && maxSeconds > 0f) oxygenSlider.value = Mathf.Clamp01(secondsLeft / maxSeconds);
    }

    public void SetObjectiveText(string text)
    {
        if (objectiveText) objectiveText.text = text ?? string.Empty;
    }
    public void SetObjective(string text) => SetObjectiveText(text);

    public void ShowPrompt(string text)
    {
        if (promptText)  promptText.text = text ?? "";
        ShowGroup(promptPanel);
    }

    public void HidePrompt()
    {
        HideGroup(promptPanel);
    }

    public void ShowWinScreen()
    {
        ShowGroup(winScreen);
        HideGroup(deathScreen);
        UnlockCursor();
        Log("[WinGameUI] WIN shown");
    }

    public void ShowDeathScreen()
    {
        ShowGroup(deathScreen);
        HideGroup(winScreen);
        UnlockCursor();
        Log("[EndGameUI] DEATH shown");
    }

    private static void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f; 
    }

    public void HideAllScreens()
    {
        HideGroup(winScreen);
        HideGroup(deathScreen);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
