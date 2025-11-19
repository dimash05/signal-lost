using UnityEngine;

public class CompassArrow : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform target;
    [SerializeField] RectTransform arrow;

    [SerializeField] bool hideWhenClose = true;
    [SerializeField] float closeDistance = 2f;

    [Header("Orientation")]
    [SerializeField] float spriteUpOffset = 180f;
    [SerializeField] bool mirrorLeftRight = false;

    void Reset()
    {
        arrow = GetComponent<RectTransform>();
    }

    void Awake()
    {
        if (!arrow) arrow = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!playerCamera || !target || !arrow) return;

        Vector3 camFwd = playerCamera.forward; camFwd.y = 0f;
        if (camFwd.sqrMagnitude < 1e-6f) camFwd = Vector3.forward;
        camFwd.Normalize();

        Vector3 toTgt = target.position - playerCamera.position; toTgt.y = 0f;
        float dist = toTgt.magnitude;

        if (hideWhenClose && dist < closeDistance)
        {
            if (arrow.gameObject.activeSelf) arrow.gameObject.SetActive(false);
            return;
        }
        if (!arrow.gameObject.activeSelf) arrow.gameObject.SetActive(true);

        if (dist > 0.001f)
        {
            toTgt.Normalize();
            float signed = Vector3.SignedAngle(camFwd, toTgt, Vector3.up);
            if (mirrorLeftRight) signed = -signed;
            arrow.localRotation = Quaternion.Euler(0f, 0f, spriteUpOffset - signed);
        }
    }

    public void SetTarget(Transform t) => target = t;
    public void SetCamera(Transform cam) => playerCamera = cam;
}
