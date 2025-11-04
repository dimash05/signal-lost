using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] Transform playerBody; 
    [SerializeField] Transform playerCamera;
    [SerializeField] float sensitivity = 120f;
    [SerializeField] float minPitch = -70f;
    [SerializeField] float maxPitch = 80f;

    float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        playerCamera.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }
}
