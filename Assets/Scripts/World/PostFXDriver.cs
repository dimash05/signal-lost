using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostFXDriver : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] PlayerFlashlight flashlight;

    Vignette vignette; FilmGrain grain; ChromaticAberration ca; Bloom bloom; ColorAdjustments col;

    void Awake()
    {
        if (!volume) volume = FindObjectOfType<Volume>();
        if (volume && volume.profile)
        {
            volume.profile.TryGet(out vignette);
            volume.profile.TryGet(out grain);
            volume.profile.TryGet(out ca);
            volume.profile.TryGet(out bloom);
            volume.profile.TryGet(out col);
        }
        if (!flashlight) flashlight = FindObjectOfType<PlayerFlashlight>();
    }

    void Update()
    {
        if (!flashlight) return;
        float inv = 1f - flashlight.Battery01;
        if (vignette) vignette.intensity.Override(Mathf.Lerp(0.25f, 0.45f, inv));
        if (grain)    grain.intensity.Override(Mathf.Lerp(0.15f, 0.45f, inv));
        if (ca)       ca.intensity.Override(Mathf.Lerp(0.03f, 0.22f, inv));
        if (bloom)    bloom.intensity.Override(Mathf.Lerp(2.5f, 6f, inv));
        if (col)      col.saturation.Override(Mathf.Lerp(-10f, -35f, inv));
    }
}
