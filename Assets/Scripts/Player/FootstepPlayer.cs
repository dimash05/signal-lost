using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AudioSource))]
public class FootstepPlayer : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip[] sand;
    public AudioClip[] rock;
    public AudioClip[] metal;

    [Header("Tuning")]
    public float stepDistance = 2.2f;
    public float rayLength = 1.6f;

    CharacterController cc; AudioSource au;
    Vector3 last; float acc;

    void Awake(){ cc = GetComponent<CharacterController>(); au = GetComponent<AudioSource>(); last = transform.position; }

    void Update()
    {
        Vector3 a = transform.position; a.y = 0;
        Vector3 b = last;              b.y = 0;
        acc += Vector3.Distance(a, b);
        last = transform.position;

        if (!cc.isGrounded || cc.velocity.magnitude < 0.15f) return;
        if (acc < stepDistance) return;

        acc = 0f;
        var type = DetectSurface();
        var bank = type==SurfaceType.Rock ? rock : type==SurfaceType.Metal ? metal : sand;
        if (bank != null && bank.Length>0) au.PlayOneShot(bank[Random.Range(0, bank.Length)], 0.7f);
    }

    SurfaceType DetectSurface()
    {
        if (Physics.Raycast(transform.position + Vector3.up*0.2f, Vector3.down, out var hit, rayLength))
        {
            var tag = hit.collider.GetComponent<SurfaceTag>() ?? hit.collider.GetComponentInParent<SurfaceTag>();
            if (tag) return tag.type;
        }
        return SurfaceType.Sand;
    }
}
