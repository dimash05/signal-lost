using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteriorZone : MonoBehaviour
{
    void Reset() => GetComponent<Collider>().isTrigger = true;

    void OnTriggerEnter(Collider other)
        => other.GetComponentInParent<PlayerLifeSupport>()?.SetInside(true);

    void OnTriggerExit(Collider other)
        => other.GetComponentInParent<PlayerLifeSupport>()?.SetInside(false);
}
