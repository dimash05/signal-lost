using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SupplyStation : MonoBehaviour
{
    [SerializeField] private float oxygenRechargePerSecond = 6f;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        var life = other.GetComponentInParent<PlayerLifeSupport>() ?? other.GetComponent<PlayerLifeSupport>();
        if (life) life.AddOxygen01(oxygenRechargePerSecond * Time.deltaTime);
    }
}
