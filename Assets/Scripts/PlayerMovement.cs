using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float mouseSensitivity = 100f;
    public Transform playerCamera;
    public CharacterController controller;
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip woodStep;
    public AudioClip stoneStep;

    private string currentSurface = "Default";
    private float verticalVelocity;
    public float gravity = -9.81f;
    private float xRotation = 0f;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);
        animator.SetBool("isWalking", move.magnitude > 0.1f);

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Wood"))
        {
            currentSurface = "Wood";
        }
        else if (hit.collider.CompareTag("Stone"))
        {
            currentSurface = "Stone";
        }
    }

    public void PlayFootstep()
    {
        switch (currentSurface)
        {
            case "Wood":
                audioSource.pitch = Random.Range(0.6f, 0.9f);
                audioSource.PlayOneShot(woodStep, Random.Range(0.8f, 1.4f));
                break;
            case "Stone":
                audioSource.pitch = Random.Range(0.6f, 0.9f);
                audioSource.PlayOneShot(stoneStep, Random.Range(0.8f, 1.4f));
                break;
            default:
                break;
        }
    }
}
