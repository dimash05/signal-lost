using UnityEngine;

[CreateAssetMenu(menuName = "SignalLost/Suit Config", fileName = "SuitConfig")]
public class SuitConfigSO : ScriptableObject
{
    [Header("Oxygen")]
    [Tooltip("How many seconds of full oxygen does the suit have?")]
    public float maxOxygenSeconds = 100f;

    [Tooltip("Oxygen consumption rate (sec/sec) outside the safe zone")]
    public float oxygenDrainPerSecond = 1.0f;

    [Tooltip("Oxygen recovery rate (sec/sec) at the station/in the interior")]
    public float oxygenRechargePerSecond = 6.0f;

    [Header("Flashlight (for the future, you don't need to touch it)")]
    public float maxBatterySeconds = 180f;
    public float batteryDrainPerSecond = 1.0f;
    public float batteryRechargePerSecond = 10f;
}
