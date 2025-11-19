using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform playerCamera;

    [Header("Movement")]
    [SerializeField] private float speed = 4.5f;
    [SerializeField] private float gravity = -20f;    
    [SerializeField] private float jumpHeight = 1.2f;

    private Vector3 velocity;

    void Awake()
    {
        if (!controller) controller = GetComponent<CharacterController>();
        if (!playerCamera)
        {
            var cam = Camera.main ? Camera.main.transform : GetComponentInChildren<Camera>()?.transform;
            if (cam) playerCamera = cam;
        }
    }

    void Update()
    {
        if (!controller) return;

        bool grounded = controller.isGrounded;
        if (grounded && velocity.y < 0f) velocity.y = -2f; 

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move;
        if (playerCamera) 
        {
            Vector3 camFwd = playerCamera.forward; camFwd.y = 0f; camFwd.Normalize();
            Vector3 camRight = playerCamera.right; camRight.y = 0f; camRight.Normalize();
            move = (camRight * h + camFwd * v).normalized;
        }
        else 
        {
            move = new Vector3(h, 0f, v).normalized;
            move = transform.TransformDirection(move);
        }

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && grounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
