using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform playerBody;   
    [SerializeField] private Transform playerCamera;  
    [SerializeField] private float sensitivity = 120f;
    [SerializeField] private float minPitch = -70f;
    [SerializeField] private float maxPitch = 80f;

    private float pitch;

    void Awake()
    {
        if (!playerBody)  playerBody  = transform;
        if (!playerCamera)
        {
            var cam = Camera.main ? Camera.main.transform : GetComponentInChildren<Camera>()?.transform;
            if (cam) playerCamera = cam;
        }
    }

    void Start()
    {
        if (playerCamera) pitch = playerCamera.localEulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        if (playerBody) playerBody.Rotate(Vector3.up, mouseX);

        if (playerCamera)
        {
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            var e = playerCamera.localEulerAngles;
            e.x = pitch; e.y = 0f; e.z = 0f;
            playerCamera.localEulerAngles = e;
        }
    }
}
