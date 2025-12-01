using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToastUI : MonoBehaviour
{
    public static ToastUI Instance { get; private set; }

    [Header("Refs")]
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TMP_Text label;

    [Header("Timing")]
    [SerializeField, Min(0f)] private float fadeIn = 0.18f;
    [SerializeField, Min(0f)] private float hold = 2.2f;
    [SerializeField, Min(0f)] private float fadeOut = 0.22f;

    [Header("Behavior")]
    [SerializeField] private bool queueMessages = true;

    readonly Queue<Request> _queue = new();
    readonly HashSet<string> _shownKeysThisSession = new();
    Coroutine _runner;

    struct Request
    {
        public string key;          // null = no dedupe
        public string message;
        public float duration;      // <= 0 => use default hold
        public bool rememberInPrefs;
    }

    #region Singleton & setup
    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (!group) group = GetComponent<CanvasGroup>();
        if (group)
        {
            group.alpha = 0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

    static bool Ensure()
    {
        if (Instance) return true;
        Instance = FindFirstObjectByType<ToastUI>(FindObjectsInactive.Include);
        return Instance != null;
    }
    #endregion

    #region Public API (static convenience)
    public static void Show(string message, float duration = -1f)
    {
        if (!Ensure()) return;
        Instance.Enqueue(null, message, duration, false);
    }

    /// <summary>Show only once per session (or persist via PlayerPrefs if rememberAcrossRuns = true).</summary>
    public static void ShowOnce(string key, string message, float duration = -1f, bool rememberAcrossRuns = false)
    {
        if (!Ensure()) return;
        // global no-repeat if requested
        if (rememberAcrossRuns && PlayerPrefs.GetInt("toast__" + key, 0) == 1) return;
        // session no-repeat
        if (Instance._shownKeysThisSession.Contains(key)) return;

        Instance.Enqueue(key, message, duration, rememberAcrossRuns);
    }

    public static void ClearQueue()
    {
        if (!Ensure()) return;
        Instance._queue.Clear();
    }
    #endregion

    #region Core
    void Enqueue(string key, string message, float duration, bool rememberInPrefs)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        _queue.Enqueue(new Request
        {
            key = key,
            message = message,
            duration = duration,
            rememberInPrefs = rememberInPrefs
        });

        if (_runner == null) _runner = StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        while (_queue.Count > 0)
        {
            var r = _queue.Dequeue();

            if (!string.IsNullOrEmpty(r.key))
            {
                _shownKeysThisSession.Add(r.key);
                if (r.rememberInPrefs) PlayerPrefs.SetInt("toast__" + r.key, 1);
            }

            if (label) label.text = r.message;

            yield return Fade(0f, 1f, fadeIn);
            float holdTime = (r.duration > 0f) ? r.duration : hold;
            yield return new WaitForSecondsRealtime(holdTime);
            yield return Fade(1f, 0f, fadeOut);
        }

        _runner = null;
    }

    IEnumerator Fade(float from, float to, float time)
    {
        if (!group || time <= 0f)
        {
            if (group) group.alpha = to;
            yield break;
        }

        group.blocksRaycasts = false;
        float t = 0f;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(from, to, t / time);
            yield return null;
        }
        group.alpha = to;
    }
    #endregion
}
