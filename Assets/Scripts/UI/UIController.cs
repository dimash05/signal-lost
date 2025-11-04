using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    [SerializeField] private Slider batterySlider;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetBattery01(float t01)
    {
        if (batterySlider) batterySlider.value = Mathf.Clamp01(t01);
    }

    public void SetPrompt(string _) {}
    public void SetObjective(string _) {}
}
