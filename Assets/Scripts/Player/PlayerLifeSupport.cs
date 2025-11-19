using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PlayerLifeSupport : MonoBehaviour
{
    [SerializeField] SuitConfigSO config;
    [SerializeField] AudioSource breath;

    [Header("Death Screen")]
    [SerializeField] CanvasGroup deathScreen;  
    [SerializeField] TMP_Text   deathText;     

    public bool IsInside { get; private set; } = true;

    float o2;
    bool dead;

    void Start()
    {
        o2 = config ? config.maxO2 : 100f;
        UpdateUI();
    }

    public void SetInside(bool v) => IsInside = v;

    void Update()
    {
        if (dead || config == null) return;

        float delta = (IsInside ? config.rechargeInsidePerSec : -config.drainOutsidePerSec) * Time.deltaTime;
        o2 = Mathf.Clamp(o2 + delta, 0f, config.maxO2);

        if (breath)
        {
            float t = 1f - (o2 / config.maxO2);
            breath.volume = Mathf.Lerp(0.05f, 0.3f, t);
            breath.pitch  = Mathf.Lerp(1.0f, 0.9f,  t);
        }

        UpdateUI();

        if (o2 <= 0f) StartCoroutine(DieAndRestart());
    }

    void UpdateUI() => UIController.Instance?.SetOxygen01(config ? o2 / config.maxO2 : 1f);

    IEnumerator DieAndRestart()
    {
        dead = true;
        var mv = GetComponent<PlayerMovement>(); if (mv) mv.enabled = false;
        var ml = GetComponent<MouseLook>();      if (ml) ml.enabled = false;

        if (deathScreen)
        {
            for (float t = 0; t < 1f; t += Time.deltaTime / 1.5f)
            {
                deathScreen.alpha = t;
                yield return null;
            }
        }

        if (deathText) deathText.text = "OXYGEN ENDED\nPress R for restart";

        while (!Input.GetKeyDown(KeyCode.R)) yield return null;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
