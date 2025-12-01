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

    bool lowOxyToastShown = false;

    public bool IsDead => isDead;
    public float Oxygen01 => Mathf.Clamp01(oxygen / Mathf.Max(1f, maxOxygenSeconds));

    private void Awake()
    {
        isInside = startInside;
        oxygen   = maxOxygenSeconds;
        PushUI();
    }

    private void Update()
    {
        if (isDead) return;

        if (!isInside && oxygenDrainPerSecond > 0f)
        {
            oxygen = Mathf.Max(0f, oxygen - oxygenDrainPerSecond * Time.deltaTime);

            if (!lowOxyToastShown && Oxygen01 <= 0.30f)
            {
                lowOxyToastShown = true;
                ToastUI.ShowOnce("low_oxy", "Oxygen low — reach a charging station!", 2.6f);
            }

            if (oxygen <= 0f)
            {
                isDead = true;
                UIController.Instance?.ShowDeathScreen();
                ToastUI.Show("You ran out of oxygen.", 2.0f);
            }

            PushUI();
        }
    }

    public void SetInside(bool value) => isInside = value;

    public void AddOxygenSeconds(float seconds)
    {
        if (isDead || seconds <= 0f) return;
        oxygen = Mathf.Clamp(oxygen + seconds, 0f, maxOxygenSeconds);
        PushUI();
    }

    public void AddOxygen01(float seconds) => AddOxygenSeconds(seconds);

    public void Refill()
    {
        if (isDead) return;
        oxygen = maxOxygenSeconds;
        PushUI();
    }

    private void PushUI()
    {
        if (UIController.Instance)
            UIController.Instance.SetOxygenSeconds(oxygen, maxOxygenSeconds);

        if (oxygenSlider) oxygenSlider.value = Oxygen01;
        if (oxygenText)   oxygenText.text   = $"{Mathf.CeilToInt(oxygen)}s";
    }
}
