using UnityEngine;
using Unity.Cinemachine;

public class PlayerFlashlight : MonoBehaviour
{
    [SerializeField] private FlashlightConfigSO config;
    [SerializeField] private Light flashlight;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    [Header("Low battery shake")]
    [SerializeField] private CinemachineImpulseSource impulse;
    [SerializeField] private float lowThreshold = 0.15f;

    [Header("Close-up dimming")]
    [SerializeField] private Transform rayOrigin;     
    [SerializeField] private float falloffStart = 0.9f;     
    [SerializeField] private float falloffEnd   = 0.25f;     
    [SerializeField, Range(0f,1f)] private float minCloseMultiplier = 0.25f; 
    [SerializeField] private LayerMask falloffMask = ~0;     

    float baseIntensity;
    float energy;
    bool isCharging;
    float shakeCooldown;

    public float Battery01 => config ? Mathf.Clamp01(energy / config.maxEnergy) : 1f;

    void Start()
    {
        if (!config) Debug.LogWarning("PlayerFlashlight: assign FlashlightConfigSO.");
        if (flashlight && config)
        {
            flashlight.spotAngle = config.spotAngle;
            flashlight.range     = config.range;
            flashlight.intensity = config.intensity;
            flashlight.enabled   = false;
        }
        baseIntensity = flashlight ? flashlight.intensity : (config ? config.intensity : 1000f);
        energy = config ? config.maxEnergy : 100f;
        UpdateUI();
        if (!rayOrigin && flashlight) rayOrigin = flashlight.transform;
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

        if (config && Battery01 <= lowThreshold)
        {
            shakeCooldown -= Time.deltaTime;
            if (shakeCooldown <= 0f && impulse != null)
            {
                impulse.GenerateImpulse();
                shakeCooldown = 1.5f;
            }
        }

        
        if (isCharging && config && energy < config.maxEnergy)
        {
            energy = Mathf.Min(config.maxEnergy, energy + config.rechargePerSecond * Time.deltaTime);
            UpdateUI();
        }

        ApplyCloseUpDimming();
    }

    void ApplyCloseUpDimming()
    {
        if (!flashlight || !flashlight.enabled) return;

        Transform o = rayOrigin ? rayOrigin : flashlight.transform;
        float mul = 1f;

        if (Physics.Raycast(o.position, o.forward, out var hit, falloffStart, falloffMask, QueryTriggerInteraction.Ignore))
        {
            float t = Mathf.InverseLerp(falloffStart, falloffEnd, hit.distance); 
            mul = Mathf.Lerp(1f, minCloseMultiplier, t);
        }

        flashlight.intensity = baseIntensity * mul;
    }

    public void SetCharging(bool v) => isCharging = v;

    void UpdateUI()
    {
        if (config) UIController.Instance?.SetBattery01(Battery01);
    }
}
