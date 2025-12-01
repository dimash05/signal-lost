using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Refs")]
    [Tooltip("Transforms the player's CAMERA - it emits a beam for targeting.")]
    public Transform playerCamera;

    [Header("Raycast")]
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private LayerMask interactMask = ~0; 

    [Header("UI (optional)")]
    [SerializeField] private TMP_Text promptText;     
    [SerializeField] private GameObject promptRoot;   
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private IInteractable _hover;

    private void Awake()
    {
        if (promptRoot != null) promptRoot.SetActive(false);
    }

    private void Update()
    {
        UpdateHover();

        if (_hover != null && Input.GetKeyDown(interactKey))
        {
            _hover.Interact(this);
        }
    }

    private void UpdateHover()
    {
        IInteractable newTarget = null;

        if (playerCamera != null)
        {
            if (Physics.Raycast(playerCamera.position, playerCamera.forward,
                                out RaycastHit hit, maxDistance, interactMask,
                                QueryTriggerInteraction.Collide))
            {
                newTarget = hit.collider.GetComponentInParent<IInteractable>()
                            ?? hit.collider.GetComponent<IInteractable>();
            }
        }

        if (!ReferenceEquals(newTarget, _hover))
        {
            _hover = newTarget;
            UpdatePrompt();
        }
    }

    private void UpdatePrompt()
    {
        if (promptRoot == null && promptText == null) return;

        if (_hover == null)
        {
            if (promptRoot != null) promptRoot.SetActive(false);
            if (promptText != null) promptText.text = string.Empty;
            return;
        }

        if (promptRoot != null) promptRoot.SetActive(true);
        if (promptText != null)
        {
            var key = interactKey.ToString();
            promptText.text = $"{key}: {_hover.Prompt}";
        }
    }
}
