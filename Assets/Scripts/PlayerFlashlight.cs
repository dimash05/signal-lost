using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Light flashlight;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    [Header("Battery")]
    [SerializeField] private float capacity = 150f;     
    [SerializeField] private float drainPerSecond = 1.0f;
    [SerializeField] private float rechargePerSecond = 10f;

    float energy; 
    bool isCharging;

    public float Battery01 => Mathf.Clamp01(energy / Mathf.Max(1f, capacity));
    public bool IsOn => flashlight && flashlight.enabled;

    void Start()
    {
        if (!flashlight) flashlight = GetComponentInChildren<Light>(true);
        energy = capacity;
        UIController.Instance?.SetBattery(Battery01);
        ApplyIntensity();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            Toggle();

        if (IsOn && !isCharging)
        {
            energy = Mathf.Max(0f, energy - drainPerSecond * Time.deltaTime);
            if (energy <= 0f) flashlight.enabled = false;
        }

        if (isCharging)
            AddEnergy(rechargePerSecond * Time.deltaTime);

        UIController.Instance?.SetBattery(Battery01);
        ApplyIntensity();
    }

    public void SetCharging(bool value) => isCharging = value;

    public void AddEnergy01(float amount) => AddEnergy(amount);
    public void AddEnergy(float amount)
    {
        energy = Mathf.Clamp(energy + amount, 0f, capacity);
    }

    void Toggle()
    {
        if (!flashlight) return;
        if (energy <= 0f) return;
        flashlight.enabled = !flashlight.enabled;
    }

    void ApplyIntensity()
    {
        if (!flashlight) return;
        flashlight.intensity = Mathf.Lerp(0.25f, 1.0f, Battery01);
        flashlight.range     = Mathf.Lerp(5f, 18f, Battery01);
    }
}
