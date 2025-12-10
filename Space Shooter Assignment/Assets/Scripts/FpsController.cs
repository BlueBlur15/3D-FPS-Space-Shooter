using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FpsController : MonoBehaviour
{
    [Header("Look")]
    public Transform playerCamera;          // assign Main Camera (child of Player)
    public float mouseSensitivity = 2.0f;
    public float minPitch = -90f;
    public float maxPitch = 90f;

    [Header("Move")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public bool enableSprint = true;

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.2f;         // meters
    public float gravity = -20f;            // more negative = snappier
    public float groundedStick = -2f;       // tiny downward "stick" on slopes

    [Header("Footstep SFX")]
    public float stepInterval = 0.5f;       // base interval for walk steps (tweak)
    
    public bool controlsLocked = false;

    private CharacterController controller;
    private float cameraPitch = 0f;
    private float verticalVelocity = 0f;
    private bool wasGroundedLastFrame = true;
    private float stepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (controlsLocked) return;

        HandleLook();
        HandleMoveAndJump();
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player horizontally (yaw)
        transform.Rotate(Vector3.up * mouseX);

        // Tilt camera vertically (pitch)
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
        if (playerCamera != null)
            playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void HandleMoveAndJump()
    {
        // WASD input
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // Choose speed (Shift to sprint)
        bool sprinting = enableSprint && Input.GetKey(KeyCode.LeftShift);
        float speed = sprinting ? sprintSpeed : walkSpeed;

        // Movement relative to facing direction
        Vector3 move = (transform.right * inputX + transform.forward * inputZ);
        if (move.sqrMagnitude > 1f) move.Normalize();

        // --- Ground / jump / land ---
        if (controller.isGrounded)
        {
            // Land sound (played once on touchdown)
            if (!wasGroundedLastFrame)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.landSound);
            }

            verticalVelocity = groundedStick;

            if (Input.GetButtonDown("Jump")) // Space
            {
                // v = sqrt(2 * jumpHeight * -gravity)
                verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);
                AudioManager.instance.PlaySFX(AudioManager.instance.jumpSound);
            }
        }
        else
        {
            // Apply gravity while airborne
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Compose final velocity and move
        Vector3 velocity = move * speed;
        velocity.y = verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        // --- Footsteps (walk/sprint) ---
        bool movingOnGround = controller.isGrounded && (Mathf.Abs(inputX) > 0.1f || Mathf.Abs(inputZ) > 0.1f);
        if (movingOnGround)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                if (sprinting)
                    AudioManager.instance.PlaySFX(AudioManager.instance.sprintSound);
                else
                    AudioManager.instance.PlaySFX(AudioManager.instance.walkSound);

                // Faster steps when sprinting (scale by relative speed)
                float speedScale = speed / Mathf.Max(0.01f, walkSpeed);
                stepTimer = stepInterval / speedScale;
            }
        }
        else
        {
            stepTimer = 0f; // reset when not moving/grounded
        }

        // remember grounded state for landing detection next frame
        wasGroundedLastFrame = controller.isGrounded;
    }

    public void SetControlsLocked(bool locked)
    {
        controlsLocked = locked;

        if (locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
