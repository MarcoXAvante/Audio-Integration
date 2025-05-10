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

    [Header("Audio")]
    public AudioSource footstepSource;
    public AudioClip woodFootstepClip;
    public AudioClip stoneFootstepClip;
    public float footstepInterval = 0.5f;
    public float surfaceCheckDistance = 1.5f;

    CharacterController cc;
    float yaw;
    float pitch;
    float verticalVelocity;
    float footstepTimer = 0f;

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
    AudioClip GetFootstepClipBySurface()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, surfaceCheckDistance))
        {
            // By tag
            if (hit.collider.CompareTag("Wood"))
                return woodFootstepClip;
            if (hit.collider.CompareTag("Stone"))
                return stoneFootstepClip;

            // Or by material name (if using PhysicMaterials)
            /*
            if (hit.collider.sharedMaterial != null)
            {
                string matName = hit.collider.sharedMaterial.name;
                if (matName.Contains("Wood")) return woodFootstepClip;
                if (matName.Contains("Stone")) return stoneFootstepClip;
            }
            */
        }
        return null; // Fallback if no match
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

        bool isMoving = new Vector2(h, v).sqrMagnitude > 0.01f;
        if (cc.isGrounded && isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                AudioClip clipToPlay = GetFootstepClipBySurface();
                if (clipToPlay != null)
                    footstepSource.PlayOneShot(clipToPlay);

                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f; 
        }

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
