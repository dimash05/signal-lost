using UnityEngine;
using UnityEngine.Events;

public class SimpleEndGameUI : MonoBehaviour
{
    public static SimpleEndGameUI Instance { get; private set; }

    [Header("Screens (disabled by default)")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject deathScreen;

    [Header("What to do on show")]
    [SerializeField] private bool pauseTime = true;
    [SerializeField] private UnityEvent onWin;
    [SerializeField] private UnityEvent onDeath;

    private bool _shown;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (winScreen)   winScreen.SetActive(false);
        if (deathScreen) deathScreen.SetActive(false);
        _shown = false;
    }

    public void ShowWin()
    {
        if (_shown) return;
        _shown = true;

        if (winScreen) winScreen.SetActive(true);
        if (pauseTime) Time.timeScale = 0f;
        onWin?.Invoke();
        Debug.Log("[EndGameUI] WIN shown");
    }

    public void ShowDeath()
    {
        if (_shown) return;
        _shown = true;

        if (deathScreen) deathScreen.SetActive(true);
        if (pauseTime) Time.timeScale = 0f;
        onDeath?.Invoke();
        Debug.Log("[EndGameUI] DEATH shown");
    }
}
