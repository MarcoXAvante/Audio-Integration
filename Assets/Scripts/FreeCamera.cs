using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FreeCameraController : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 1f;
    public float gravity = 9.81f;
    public float stepDownBias = 0.1f;

    [Header("Look")]
    public float lookSensitivity = 2f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    CharacterController cc;
    float yaw;
    float pitch;
    float verticalVelocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleMovementWithGravity();
    }

    void HandleLook()
    {
        yaw += Input.GetAxis("Mouse X") * lookSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * lookSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }

    void HandleMovementWithGravity()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 move = (right * h + forward * v) * movementSpeed;

        if (cc.isGrounded)
        {
            verticalVelocity = -stepDownBias;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        move.y = verticalVelocity;

        cc.Move(move * Time.deltaTime);
    }
}
