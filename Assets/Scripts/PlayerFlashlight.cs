using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Light flashlight;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] private FlashlightConfigSO config;

    [Header("Battery (fallback if no config)")]
    [SerializeField] private float capacity = 150f;
    [SerializeField] private float drainPerSecond = 1.0f;
    [SerializeField] private float rechargePerSecond = 10f;

    [Header("Light curve (fallback if no config)")]
    [SerializeField] private float maxIntensity = 150f;
    [SerializeField] private float maxRange = 25f;
    [SerializeField, Range(0f, 1f)] private float minIntensityFactor = 0.25f;
    [SerializeField, Range(0f, 1f)] private float minRangeFactor = 0.30f;
    [SerializeField] private float spotAngle = 70f;

    private float energy;
    private bool isCharging;

    private bool lowBatteryToastShown = false;
    private bool chargingToastShown   = false;

    public float Battery01 => Mathf.Clamp01(energy / Mathf.Max(1f, capacity));
    public bool IsOn => flashlight && flashlight.enabled;

    void Reset() { flashlight = GetComponentInChildren<Light>(true); }

    void Start()
    {
        if (!flashlight) flashlight = GetComponentInChildren<Light>(true);
        if (config) ApplyConfig(config);
        ApplyLightStatics();

        energy = Mathf.Clamp(energy == 0f ? capacity : energy, 0f, capacity);
        UIController.Instance?.SetBattery(Battery01);
        ApplyIntensity();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey)) Toggle();

        if (IsOn && !isCharging)
        {
            energy = Mathf.Max(0f, energy - drainPerSecond * Time.deltaTime);
            if (energy <= 0f && flashlight) flashlight.enabled = false;

            if (!lowBatteryToastShown && Battery01 <= 0.25f)
            {
                lowBatteryToastShown = true;
                ToastUI.ShowOnce("low_batt", "Flashlight battery low — find a charger.", 2.6f);
            }
        }

        if (isCharging)
        {
            AddEnergy(rechargePerSecond * Time.deltaTime);
        }

        UIController.Instance?.SetBattery(Battery01);
        ApplyIntensity();
    }

    public void SetCharging(bool value)
    {
        if (value && !isCharging && !chargingToastShown)
        {
            ToastUI.Show("Recharging flashlight…", 1.6f);
            chargingToastShown = true;
        }
        if (!value && isCharging)
        {
            ToastUI.Show("Flashlight charged.", 1.6f);
        }
        isCharging = value;
    }

    public void AddEnergy(float amount)
    {
        energy = Mathf.Clamp(energy + amount, 0f, capacity);
    }
    public void AddEnergy01(float amount01) => AddEnergy(amount01);

    void Toggle()
    {
        if (!flashlight) return;
        if (energy <= 0f) return;
        flashlight.enabled = !flashlight.enabled;
    }

    void ApplyIntensity()
    {
        if (!flashlight) return;
        var minI = maxIntensity * Mathf.Clamp01(minIntensityFactor);
        var minR = maxRange * Mathf.Clamp01(minRangeFactor);
        flashlight.intensity = Mathf.Lerp(minI, maxIntensity, Battery01);
        flashlight.range     = Mathf.Lerp(minR, maxRange,     Battery01);
    }

    void ApplyLightStatics()
    {
        if (flashlight) flashlight.spotAngle = spotAngle;
    }

    public void ApplyConfig(FlashlightConfigSO c)
    {
        if (c == null) return;
        config = c;

        capacity           = c.capacity;
        drainPerSecond     = c.drainPerSecond;
        rechargePerSecond  = c.rechargePerSecond;
        spotAngle          = c.spotAngle;
        maxRange           = c.maxRange;
        maxIntensity       = c.maxIntensity;
        minIntensityFactor = c.minIntensityFactor;
        minRangeFactor     = c.minRangeFactor;

        ApplyLightStatics();
        ApplyIntensity();
    }

    public void SetConfigRef(FlashlightConfigSO c) { config = c; }
    public Light GetLight() => flashlight;
}
