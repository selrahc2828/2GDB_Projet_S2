using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public Transform playerBody;
    public Transform playerCamera;

    public float walkSpeed = 3.0f;
    public float sprintSpeed = 6.0f;
    public float jumpForce = 8.0f;
    public float gravity = 30.0f;
    public float crouchHeight = 0.5f;
    public float playerHeight = 2.0f;
    public float rotationSpeed = 10f;


    private bool isGrounded;
    private bool isSprinting;

    private Vector3 velocity;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float cameraRotation = playerCamera.eulerAngles.y;

        Vector3 moveDirection = Quaternion.Euler(0f, cameraRotation, 0f) * new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Apply movement
        float speed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 targetVelocity = playerBody.TransformDirection(moveDirection) * speed;

        

        // Apply gravity
        if (!characterController.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2.0f * gravity);
            }
        }

        characterController.Move(targetVelocity * Time.deltaTime + velocity * Time.deltaTime);
    }
}
