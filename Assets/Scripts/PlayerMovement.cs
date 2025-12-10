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
    public float stopSmoothness = 8f;

    [Header("Sprint")]
    public bool unlimitedSprint = false;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintDuration = 5f;
    public float sprintCooldown = 0.5f;
    public bool useSprintBar = true;
    public Slider sprintSlider;
    private float sprintRemaining;
    private bool isSprinting = false;
    private bool canSprint = true;

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

    [Header("Audio")]
    public AudioSource walkAudio;
    public AudioSource runAudio;
    public AudioSource ambienceAudio;

    [HideInInspector]
    public bool inputLocked = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        originalScale = transform.localScale;

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
        yaw = transform.eulerAngles.y;
        pitch = playerCamera.transform.localEulerAngles.x;
        if(ambienceAudio != null && !ambienceAudio.isPlaying)
        {
            ambienceAudio.loop = true;
            ambienceAudio.Play();
        }
    }

    private void Update()
    {
        if (inputLocked) return;

        // --- Mouse Look ---
        //yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        //pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        //pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        //transform.rotation = Quaternion.Euler(0, yaw, 0);
        //playerCamera.transform.localRotation = Quaternion.Euler(pitch, 120f, 0);

        // --- Jump ---
        if (Input.GetKeyDown(jumpKey) && isGrounded)
            Jump();

        // --- Crouch ---
        if (Input.GetKeyDown(crouchKey))
            ToggleCrouch();

        CheckGround();
    }

    private void FixedUpdate()
    {
        if (inputLocked)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        transform.rotation = Quaternion.Euler(0, yaw, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 120f, 0);

        // --- Movement ---
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * input.z + camRight * input.x).normalized;

        HandleSprint();

        /*if (moveDir.magnitude > 0.1f)
        {
            Vector2 lv = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).normalized;
            Vector2 inputDirection = new Vector2(moveDir.x, moveDir.z).normalized;

            if (lv != inputDirection)
            {
                float mag = lv.magnitude;
                Vector2 newVelocity = inputDirection * mag;
                rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.y);
            }
        }*/

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 targetVelocity = moveDir * currentSpeed;
        Vector3 velocity = rb.linearVelocity;
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, velocity.z);

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

        if (moveDir.magnitude > 0.1f)
        {
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 current = rb.linearVelocity;
            current.x = Mathf.Lerp(current.x, 0, Time.fixedDeltaTime * stopSmoothness);
            current.z = Mathf.Lerp(current.z, 0, Time.fixedDeltaTime * stopSmoothness);
            rb.linearVelocity = current;
        }
        HandleFootsteps(moveDir);
    }

    // --- Sprint Logic ---
    private void HandleSprint()
    {
        if (unlimitedSprint)
        {
            isSprinting = Input.GetKey(sprintKey);
            return;
        }

        bool shiftHeld = Input.GetKey(sprintKey);

        if (shiftHeld && canSprint && sprintRemaining > 0f)
        {
            isSprinting = true;
            sprintRemaining -= Time.fixedDeltaTime;

            if (sprintRemaining <= 0f)
            {
                sprintRemaining = 0f;
                canSprint = false;
                isSprinting = false;
            }
        }
        else
        {
            sprintRemaining += Time.fixedDeltaTime / sprintCooldown;
            sprintRemaining = Mathf.Clamp(sprintRemaining, 0f, sprintDuration);

            if (sprintRemaining >= sprintDuration)
                canSprint = true;

            
            isSprinting = false;
        }

        if (useSprintBar && sprintSlider != null)
            sprintSlider.value = sprintRemaining / sprintDuration;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isGrounded = false;

        if (isCrouched)
            ToggleCrouch();
    }

    private void ToggleCrouch()
    {
        if (isCrouched)
        {
            transform.localScale = originalScale;
            walkSpeed /= speedReduction;
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            walkSpeed *= speedReduction;
        }

        isCrouched = !isCrouched;
    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2f), transform.position.z);
        isGrounded = Physics.Raycast(origin, Vector3.down, 0.75f);
    }
    private void HandleFootsteps(Vector3 moveDir){
        bool isMoving = moveDir.magnitude > 0.5f;

        if(isGrounded && isMoving)
        {
            if(isSprinting)
            {
                if(!runAudio.isPlaying)
                {
                    runAudio.Play();
                }
                if (walkAudio.isPlaying){
                    walkAudio.Stop();
                }
            }
            else
            {
                if(!walkAudio.isPlaying)
                {
                    walkAudio.Play();
                }
                if (runAudio.isPlaying){
                    runAudio.Stop();
                }
            }
        }
        else
        {
            if (walkAudio.isPlaying)
            {
                walkAudio.Stop();
            }
            if (runAudio.isPlaying)
            {
                runAudio.Stop();
            }
        }
    }

    public void LockInput(bool locked)
    {
        inputLocked = locked;

        if (locked)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}