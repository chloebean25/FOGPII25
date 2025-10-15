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

    [HideInInspector]
    public bool inputLocked = false;

    private Vector3 lastPosition; 
    private Vector3 lastForward;  

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

       
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lastPosition = transform.position;
        lastForward = transform.forward;
    }

    private void Update()
    {
       
        if (inputLocked) return;

        
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        transform.rotation = Quaternion.Euler(0, yaw, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        //Jump 
        if (Input.GetKeyDown(jumpKey) && isGrounded)
            Jump();

        //Crouch 
        if (Input.GetKeyDown(crouchKey))
            ToggleCrouch();

        CheckGround();
    }

    private void FixedUpdate()
    {
        if (inputLocked) return;

        //Movement
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * input.z + camRight * input.x).normalized;

        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        if (Input.GetKey(sprintKey) && canSprint && sprintRemaining > 0f)
        {
            isSprinting = true;
            sprintRemaining -= Time.fixedDeltaTime;
        }
        else
        {
            isSprinting = false;
            sprintRemaining += Time.fixedDeltaTime;
        }

        sprintRemaining = Mathf.Clamp(sprintRemaining, 0f, sprintDuration);
        if (useSprintBar && sprintSlider != null)
            sprintSlider.value = sprintRemaining / sprintDuration;

        Vector3 targetVelocity = moveDir * currentSpeed;
        Vector3 velocity = rb.linearVelocity;
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, velocity.z);

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        
        lastPosition = transform.position;
        lastForward = transform.forward;
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

    public void LockInput(bool locked)
    {
        inputLocked = locked;

        if (locked)
        {
            
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 v = rb.linearVelocity;
        v.x = 0;
        v.z = 0;
        rb.linearVelocity = v;
    }
}
