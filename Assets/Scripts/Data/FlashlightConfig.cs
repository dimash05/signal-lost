using UnityEngine;

[CreateAssetMenu(menuName = "SignalLost/Flashlight Config", fileName = "FlashlightConfig")]
public class FlashlightConfig : ScriptableObject
{
    [Header("Energy")]
    public float maxEnergy = 100f;
    public float drainPerSecond = 6f;
    public float rechargePerSecond = 15f;

    [Header("Light")]
    public float spotAngle = 60f;
    public float range = 15f;
    public float intensity = 8000f; 
}
