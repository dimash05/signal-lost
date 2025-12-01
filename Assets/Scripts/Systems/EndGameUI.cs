using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    public static EndGameUI Instance { get; private set; }

    [Header("Canvas / Group")]
    [SerializeField] private Canvas endCanvas;       
    [SerializeField] private CanvasGroup group;     

    [Header("Roots")]
    [SerializeField] private GameObject winRoot;    
    [SerializeField] private GameObject deathRoot;   

    [Header("Texts ()")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text subtitle;

    [Header("Hide HUD")]
    [SerializeField] private Canvas hudCanvas;       

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        if (!endCanvas) endCanvas = GetComponentInParent<Canvas>();
        if (!group)     group     = GetComponent<CanvasGroup>();

        if (endCanvas)
        {
            endCanvas.renderMode   = RenderMode.ScreenSpaceOverlay;
            endCanvas.sortingOrder = 1000; 
        }

        HideImmediate(); 
    }

    public void ShowWin(string t = "Signal sent!", string s = "Rescue is on the way.")
    {
        PrepareCommon(t, s);
        if (winRoot)   winRoot.SetActive(true);
        if (deathRoot) deathRoot.SetActive(false);
        Show();
        Debug.Log("[EndGameUI] WIN shown");
    }

    public void ShowDeath(string t = "You ran out of oxygen", string s = "Try another route.")
    {
        PrepareCommon(t, s);
        if (winRoot)   winRoot.SetActive(false);
        if (deathRoot) deathRoot.SetActive(true);
        Show();
        Debug.Log("[EndGameUI] DEATH shown");
    }

    private void PrepareCommon(string t, string s)
    {
        if (hudCanvas) hudCanvas.enabled = false;    
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible    = true;
        Time.timeScale    = 0f;                      

        if (title)    title.text    = t;
        if (subtitle) subtitle.text = s;

        transform.SetAsLastSibling();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        if (group)
        {
            group.alpha          = 1f;
            group.blocksRaycasts = true;
            group.interactable   = true;
        }
    }

    public void HideImmediate()
    {
        if (group)
        {
            group.alpha          = 0f;
            group.blocksRaycasts = false;
            group.interactable   = false;
        }
        if (winRoot)   winRoot.SetActive(false);
        if (deathRoot) deathRoot.SetActive(false);

        gameObject.SetActive(true);
    }
}
