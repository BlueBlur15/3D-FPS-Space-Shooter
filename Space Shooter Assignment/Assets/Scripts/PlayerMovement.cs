using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;            // Player movement speed
    public float mouseSensitivity = 2f;     // Mouse movement sensitivity

    [Header("Jump Settings")]
    public float jumpForce = 5.5f;          // Tweak to taste
    public Transform groundCheck;           // drag your GroundCheck child here
    public float groundCheckRadius = 0.25f;
    public LayerMask groundMask;            // Set to "Ground" layer (or everything if layer is not made)

    [Header("References")]
    public Transform playerCamera;
    private Rigidbody rb;

    private float xRotation = 0f;           // Used to tilt camera up/down
    private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody from the same object
        rb = GetComponent<Rigidbody>();

        // Freeze physics rotation so the player doesn't tip over
        rb.freezeRotation = true;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseLook();
        HandleGroundCheck();
        HandleJump();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the player body (Y-axis)
        transform.Rotate(Vector3.up * mouseX);

        // Adjust vertical rotation (X-axis, camera tilt)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents flipping

        // Apply rotation to the camera only
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleGroundCheck()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
        }
        else
        {
            // Fallback: small downward ray if groundCheck is not assigned
            isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask, QueryTriggerInteraction.Ignore);
        }
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Clear any downward velocity so jumps feel snappy
            Vector3 v = rb.velocity;
            if (v.y < 0f) v.y = 0f;
            rb.velocity = v;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    void HandleMovement()
    {
        // Get WASD input
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical");   // W/S

        // Calculate direction relative to where the player is facing
        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;

        // Move using Rigidbody to stay physics-friendly
        Vector3 newPosition = rb.position + moveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);
    }
}
