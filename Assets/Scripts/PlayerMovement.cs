using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 4.5f;
    [SerializeField] float gravity = -20f;
    [SerializeField] float jumpHeight = 1.2f;

    CharacterController controller;
    float verticalVelocity;

    void Awake() => controller = GetComponent<CharacterController>();

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = (transform.right * x + transform.forward * z) * speed;

        bool isGrounded = controller.isGrounded;
        if (isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;

        if (isGrounded && Input.GetButtonDown("Jump"))
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;

        controller.Move((move + Vector3.up * verticalVelocity) * Time.deltaTime);
    }
}
