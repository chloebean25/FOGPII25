using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
   private Rigidbody rb;

    [Header("Camera")]
    public Camera playerCamera;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;
    private float yaw = 0f;
    private float pitch = 0f;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 7f;
    public float maxVelocityChange = 10f;

    [Header("Sprint")]
    public bool unlimitedSprint = false;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintDuration = 5f;
    public float sprintCooldown = 0.5f;
    public bool useSprintBar = true;
    public Slider sprintSlider;
    private float sprintRemaining;
    private bool isSprinting = false;
    private bool isSprintCooldown = false;
    private float sprintCooldownReset;

    [Header("Jump")]
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;
    private bool isGrounded = false;

    [Header("Crouch")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = 0.75f;
    public float speedReduction = 0.5f;
    private Vector3 originalScale;
    private bool isCrouched = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;

       if (!unlimitedSprint)
    {
        sprintRemaining = sprintDuration;
        sprintCooldownReset = sprintCooldown;

        if  (sprintSlider != null)
        {
         sprintSlider.minValue = 0f;
         sprintSlider.maxValue = sprintDuration;
         sprintSlider.value = sprintDuration;
        }
    }
    }
    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Camera
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        // Jump
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        // Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            ToggleCrouch();
        }

        CheckGround();
    }

    private void FixedUpdate()
    {
        // Movement
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Sprint
        if (Input.GetKey(sprintKey) && sprintRemaining > 0f && !isSprintCooldown)
        {
            targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;
            isSprinting = true;

            if (!unlimitedSprint)
            {
                sprintRemaining -= Time.fixedDeltaTime;
                sprintRemaining = Mathf.Clamp(sprintRemaining, 0f, sprintDuration);
            }
        }
        else
        {
            targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;
            isSprinting = false;

            if (!unlimitedSprint && !isSprintCooldown)
            {
                sprintRemaining = Mathf.Clamp(sprintRemaining + Time.fixedDeltaTime, 0f, sprintDuration);
            }
        }
    
        // Update sprint bar (Slider version)
        if (useSprintBar && !unlimitedSprint && sprintSlider != null)
        {
            sprintSlider.value = sprintRemaining;
        }


        // Sprint cooldown
        if (isSprintCooldown)
        {
            sprintCooldown -= Time.fixedDeltaTime;
            if (sprintCooldown <= 0f)
            {
                isSprintCooldown = false;
                sprintCooldown = sprintCooldownReset;
            }
        }

        // Apply velocity change
        Vector3 velocity = rb.linearVelocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        // Update sprint bar
        if (useSprintBar && !unlimitedSprint && sprintSlider != null)
        {
         if (useSprintBar && !unlimitedSprint && sprintSlider != null)
            {
                sprintSlider.value = sprintRemaining;
            }
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isGrounded = false;

        if (isCrouched)
        {
            ToggleCrouch();
        }
    }

    private void ToggleCrouch()
    {
        if (isCrouched)
        {
            transform.localScale = originalScale;
            walkSpeed /= speedReduction;
            isCrouched = false;
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            walkSpeed *= speedReduction;
            isCrouched = true;
        }
    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2f), transform.position.z);
        isGrounded = Physics.Raycast(origin, Vector3.down, 0.75f);
    }
}