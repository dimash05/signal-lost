using UnityEngine;
using TMPro;
using System.Collections;

public class FinalConsole : MonoBehaviour, IInteractable
{
    [SerializeField] private LoopRotate antenna;
    [SerializeField] private CanvasGroup endScreen; 
    [SerializeField] private TMP_Text   winText;   
    [SerializeField] private AudioSource au;
    [SerializeField] private AudioClip   transmit;
    [SerializeField] private float fadeTime = 2f;

    bool done;

    public string GetPrompt() => done ? "Signal already sent" : "Send signal";

    public void Interact()
    {
        if (done) return;
        done = true;

        antenna?.SetActive(true);
        if (au && transmit) au.PlayOneShot(transmit, 0.9f);

        UIController.Instance?.SetObjective("Signal sent. Help is on it's way...");
        StartCoroutine(ShowWin());
    }

    IEnumerator ShowWin()
    {
        if (endScreen)
        {
            for (float t = 0; t < 1f; t += Time.deltaTime / fadeTime)
            {
                endScreen.alpha = t;
                yield return null;
            }
            endScreen.alpha = 1f;
        }

        if (winText)
        {
            winText.text = "SIGNAL SENT\nYou won the game!";
            winText.gameObject.SetActive(true);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }
}
