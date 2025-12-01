using UnityEngine;

[CreateAssetMenu(fileName = "FlashlightConfig", menuName = "SignalLost/Flashlight Config")]
public class FlashlightConfigSO : ScriptableObject
{
    [Header("Energy")]
    public float capacity = 100f;
    public float drainPerSecond = 4f;
    public float rechargePerSecond = 15f;

    [Header("Light")]
    public float spotAngle = 70f;
    public float maxRange = 25f;
    public float maxIntensity = 150f;

    [Header("Light min factors (0..1)")]
    [Range(0f, 1f)] public float minRangeFactor = 0.30f;
    [Range(0f, 1f)] public float minIntensityFactor = 0.25f;
}
