using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask = ~0; 

    private IInteractable current;

    void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    void Update()
    {
        IInteractable found = null;

        var ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out var hit, distance, mask, QueryTriggerInteraction.Ignore))
        {
            found = hit.collider.GetComponent<IInteractable>() ??
                    hit.collider.GetComponentInParent<IInteractable>();
        }

        if (found != current)
        {
            current = found;
            UIController.Instance?.SetPrompt(current != null ? "[E] " + current.GetPrompt() : "");
        }

        if (current != null && Input.GetKeyDown(KeyCode.E))
            current.Interact();
    }
}
