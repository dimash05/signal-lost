using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChargingStation : MonoBehaviour
{
    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        var flash = other.GetComponentInChildren<PlayerFlashlight>();
        if (flash) flash.SetCharging(true);
    }

    void OnTriggerExit(Collider other)
    {
        var flash = other.GetComponentInChildren<PlayerFlashlight>();
        if (flash) flash.SetCharging(false);
    }
}
