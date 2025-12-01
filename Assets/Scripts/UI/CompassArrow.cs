using UnityEngine;

/// <summary>

/// </summary>
public class CompassArrow : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Compass target provider. If not specified, one will be found automatically..")]
    public CompassTargetProvider provider;

    [Tooltip("Player/direction anchor. Default = provider transform.")]
    public Transform player;

    [Tooltip("RectTransform the arrow itself. If it's empty, we'll take our own.")]
    public RectTransform arrow;

    [Header("Tuning")]
    [Tooltip("Turn smoothing (the higher the smoother).")]
    public float smooth = 12f;

    [Tooltip("Sprite offset in degrees. If the sprite is facing DOWN, set 180.")]
    public float spriteForwardOffset = 0f;

    [Tooltip("Hide the arrow when there is no target.")]
    public bool hideWhenNoTarget = true;

    void Awake()
    {
        if (!provider) provider = FindObjectOfType<CompassTargetProvider>(true);
        if (!player && provider) player = provider.transform;
        if (!arrow) arrow = transform as RectTransform;
    }

    void LateUpdate()
    {
        var target = provider ? provider.CurrentTarget : null;
        bool hasTarget = (target && player && arrow);

        if (hideWhenNoTarget) arrow.gameObject.SetActive(hasTarget);
        if (!hasTarget) return;

        Vector3 toTarget = target.position - player.position;
        toTarget.y = 0f;
        if (toTarget.sqrMagnitude < 0.0001f) return;

        Vector3 fwd = player.forward; 
        fwd.y = 0f;
        if (fwd.sqrMagnitude < 0.0001f) fwd = Vector3.forward;

        float angle = Vector3.SignedAngle(fwd, toTarget, Vector3.up);

        float targetZ = -angle + spriteForwardOffset;

        float z = Mathf.LerpAngle(arrow.localEulerAngles.z, targetZ, Time.deltaTime * smooth);
        arrow.localEulerAngles = new Vector3(0f, 0f, z);
    }
}
