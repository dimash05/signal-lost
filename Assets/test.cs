using UnityEngine;

public class UiQuickTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
            UIController.Instance?.ShowWinScreen();
        if (Input.GetKeyDown(KeyCode.Alpha0))
            UIController.Instance?.ShowDeathScreen();
    }
}
