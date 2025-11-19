using UnityEngine;

[CreateAssetMenu(menuName = "SignalLost/Suit Config", fileName = "SuitConfig")]
public class SuitConfigSO : ScriptableObject
{
    public float maxO2 = 100f;
    public float drainOutsidePerSec = 2f;   
    public float rechargeInsidePerSec = 4f; 
}
