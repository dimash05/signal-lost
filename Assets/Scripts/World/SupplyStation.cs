using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SupplyStation : MonoBehaviour
{
    [Header("What to recharge")]
    [SerializeField] bool rechargeBattery = true;
    [SerializeField] bool rechargeOxygen  = true;

    [Header("Rates (per second)")]
    [SerializeField] float batteryPerSecond = 15f; 
    [SerializeField] float oxygenPerSecond  = 10f; 

    [Header("UI Prompt (optional)")]
    [SerializeField] bool   showPrompt      = true;
    [SerializeField] bool   requireHoldKey  = false;
    [SerializeField] KeyCode interactKey    = KeyCode.E;
    [SerializeField] string promptText      = "Recharge (E)";

    [Header("Debug")]
    [SerializeField] bool autoAddKinematicRigidbody = true;
    [SerializeField] bool logMessages = false;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (!col) col = gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;

        if (autoAddKinematicRigidbody && !TryGetComponent<Rigidbody>(out _))
        {
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (showPrompt && UIController.Instance)
            UIController.Instance.ShowPrompt(requireHoldKey ? promptText : "Recharging...");
    }

    void OnTriggerExit(Collider other)
    {
        if (showPrompt && UIController.Instance)
            UIController.Instance.HidePrompt();
    }

    void OnTriggerStay(Collider other)
    {
        if (requireHoldKey && !Input.GetKey(interactKey)) return;

        float dt = Time.deltaTime;

        if (rechargeBattery)
        {
            var flash = other.GetComponentInParent<PlayerFlashlight>() ?? other.GetComponentInChildren<PlayerFlashlight>();
            if (flash) flash.AddEnergy(batteryPerSecond * dt);
        }

        if (rechargeOxygen)
        {
            var life = other.GetComponentInParent<PlayerLifeSupport>() ?? other.GetComponentInChildren<PlayerLifeSupport>();
            if (life) life.AddOxygen01(oxygenPerSecond * dt); 
        }

        if (logMessages) Debug.Log("[SupplyStation] recharging…");
    }
}
