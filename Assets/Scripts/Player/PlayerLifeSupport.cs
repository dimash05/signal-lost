using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLifeSupport : MonoBehaviour
{
    [Header("Oxygen settings")]
    [SerializeField] private float maxOxygenSeconds = 120f;
    [SerializeField] private float oxygenDrainPerSecond = 1f;
    [SerializeField] private bool startInside = false;

    [Header("UI (optional)")]
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private TMP_Text oxygenText;

    private float oxygen;
    private bool isInside;
    private bool isDead;

    public bool IsDead => isDead;
    public float Oxygen01 => Mathf.Clamp01(oxygen / Mathf.Max(1f, maxOxygenSeconds));

    private void Awake()
    {
        isInside = startInside;
        oxygen   = maxOxygenSeconds;
        UpdateUI();
    }

    private void Update()
    {
        if (isDead) return;

        if (!isInside && oxygenDrainPerSecond > 0f)
        {
            oxygen -= oxygenDrainPerSecond * Time.deltaTime;
            if (oxygen <= 0f)
            {
                oxygen = 0f;
                isDead = true;
                UIController.Instance?.ShowDeathScreen();
            }
            UpdateUI();
        }
    }

    public void SetInside(bool value) => isInside = value;

    public void AddOxygen01(float seconds)
    {
        if (isDead || seconds <= 0f) return;
        oxygen = Mathf.Clamp(oxygen + seconds, 0f, maxOxygenSeconds);
        UpdateUI();
    }

    public void Refill()
    {
        if (isDead) return;
        oxygen = maxOxygenSeconds;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (oxygenSlider) oxygenSlider.value = Oxygen01;
        if (oxygenText)   oxygenText.text = $"{Mathf.CeilToInt(oxygen)}s";
    }
}
