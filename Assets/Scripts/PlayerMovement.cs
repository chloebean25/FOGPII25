using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [Header("Camera")]
    public Camera playerCamera;
    public float mouseSensitivity = 300f;
    public float maxLookAngle = 80f;

    private float xRotation = 0f;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2.5f;
    public float gravity = -20f;

    private Vector3 velocity;

    [Header("Sprint")]
    public bool unlimitedSprint = false;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintDuration = 5f;
    public float sprintCooldown = 0.5f;
    public bool useSprintBar = true;
    public Slider sprintSlider;

    private float sprintRemaining;
    private bool canSprint = true;
    private bool isSprinting = false;

    [Header("Jump")]
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpHeight = 1.5f;

    [Header("Crouch")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;

    private bool isCrouched = false;

    [Header("Audio")]
    public AudioSource walkAudio;
    public AudioSource runAudio;
    public AudioSource ambienceAudio;

    [HideInInspector]
    public bool inputLocked = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (!unlimitedSprint)
        {
            sprintRemaining = sprintDuration;

            if (sprintSlider != null)
            {
                sprintSlider.minValue = 0f;
                sprintSlider.maxValue = 1f;
                sprintSlider.value = 1f;
            }
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (ambienceAudio != null && !ambienceAudio.isPlaying)
        {
            ambienceAudio.loop = true;
            ambienceAudio.Play();
        }
    }

    private void Update()
    {
        if (inputLocked)
            return;

        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleCrouch();
        HandleSprint();
        HandleFootsteps();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float currentSpeed = walkSpeed;

        if (isCrouched)
        {
            currentSpeed = crouchSpeed;
        }
        else if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }

        controller.Move(move.normalized * currentSpeed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(jumpKey) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (isCrouched)
            {
                ToggleCrouch();
            }
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            ToggleCrouch();
        }
    }

    private void ToggleCrouch()
    {
        if (isCrouched)
        {
            controller.height = standingHeight;
        }
        else
        {
            controller.height = crouchingHeight;
        }

        isCrouched = !isCrouched;
    }

    private void HandleSprint()
    {
        if (unlimitedSprint)
        {
            isSprinting = Input.GetKey(sprintKey) && !isCrouched;
            return;
        }

        bool shiftHeld = Input.GetKey(sprintKey);

        if (shiftHeld && canSprint && sprintRemaining > 0f && !isCrouched)
        {
            isSprinting = true;

            sprintRemaining -= Time.deltaTime;

            if (sprintRemaining <= 0f)
            {
                sprintRemaining = 0f;
                canSprint = false;
                isSprinting = false;
            }
        }
        else
        {
            isSprinting = false;

            sprintRemaining += Time.deltaTime / sprintCooldown;
            sprintRemaining = Mathf.Clamp(sprintRemaining, 0f, sprintDuration);

            if (sprintRemaining >= sprintDuration)
            {
                canSprint = true;
            }
        }

        if (useSprintBar && sprintSlider != null)
        {
            sprintSlider.value = sprintRemaining / sprintDuration;
        }
    }

    private void HandleFootsteps()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        bool isMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        if (controller.isGrounded && isMoving)
        {
            if (isSprinting)
            {
                if (!runAudio.isPlaying)
                    runAudio.Play();

                if (walkAudio.isPlaying)
                    walkAudio.Stop();
            }
            else
            {
                if (!walkAudio.isPlaying)
                    walkAudio.Play();

                if (runAudio.isPlaying)
                    runAudio.Stop();
            }
        }
        else
        {
            if (walkAudio.isPlaying)
                walkAudio.Stop();

            if (runAudio.isPlaying)
                runAudio.Stop();
        }
    }

    public void LockInput(bool locked)
    {
        inputLocked = locked;

        if (locked)
        {
            velocity = Vector3.zero;
        }
    }
}