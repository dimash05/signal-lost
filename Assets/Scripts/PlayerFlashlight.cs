using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
    [SerializeField] private FlashlightConfigSO config; 
    [SerializeField] private Light flashlight;         
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    private float energy;
    private bool isCharging;

    void Start()
    {
        if (!config) Debug.LogWarning("PlayerFlashlight: Assign FlashlightConfigSO in Inspector.");
        if (flashlight && config)
        {
            flashlight.spotAngle = config.spotAngle;
            flashlight.range     = config.range;
            flashlight.intensity = config.intensity;
            flashlight.enabled   = false;
        }
        energy = config ? config.maxEnergy : 100f;
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && flashlight && energy > 0f)
            flashlight.enabled = !flashlight.enabled;

        if (flashlight && flashlight.enabled && config)
        {
            energy -= config.drainPerSecond * Time.deltaTime;
            if (energy <= 0f) { energy = 0f; flashlight.enabled = false; }
            UpdateUI();
        }

        if (isCharging && config && energy < config.maxEnergy)
        {
            energy = Mathf.Min(config.maxEnergy, energy + config.rechargePerSecond * Time.deltaTime);
            UpdateUI();
        }
    }

    public void SetCharging(bool v) => isCharging = v;

    private void UpdateUI()
    {
        if (config) UIController.Instance?.SetBattery01(energy / config.maxEnergy);
    }
}
