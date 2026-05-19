using UnityEngine;
using TMPro;

public class KeyPadManager : MonoBehaviour
{
    public static KeyPadManager Instance;

    [Header("UI")]
    public GameObject keypadUI;
    public TMP_Text inputText;

    [Header("Player")]
    public PlayerMovement playerMovement;
    public GameObject player;

    [Header("Puzzle")]
    public string correctCode = "8457";
    public Transform teleportSpot;

    [Header("Optional Trigger (IMPORTANT)")]
    public Collider keypadTrigger; // assign your trigger collider here

    private string currentInput = "";
    public bool isOpen = false;

    private bool puzzleSolved = false;
    private bool justTeleported = false;

    private void Awake()
    {
        Instance = this;
        keypadUI.SetActive(false);
    }

    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
        }
    }

    public void AddDigit(string digit)
    {
        if (puzzleSolved) return;

        if (currentInput.Length < 10)
        {
            currentInput += digit;
            inputText.text = currentInput;
        }
    }

    public void ClearInput()
    {
        currentInput = "";
        inputText.text = "";
    }

    public void SubmitCode()
    {
        if (puzzleSolved) return;

        if (currentInput == correctCode)
        {
            puzzleSolved = true;

            TeleportPlayer();
            CloseKeypad();

            // ?? Prevent keypad from ever reopening
            if (keypadTrigger != null)
                keypadTrigger.enabled = false;
        }
        else
        {
            ClearInput();
        }
    }

    private void TeleportPlayer()
    {
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
            cc.enabled = false;

        player.transform.position = teleportSpot.position;

        if (cc != null)
            cc.enabled = true;

        // stop weird movement carry-over
        if (playerMovement != null)
            playerMovement.LockInput(false);

        justTeleported = true;

        // optional: small delay before allowing interactions again
        Invoke(nameof(ClearTeleportFlag), 0.5f);
    }

    private void ClearTeleportFlag()
    {
        justTeleported = false;
    }

    public void OpenKeypad()
    {
        if (isOpen || puzzleSolved || justTeleported) return;

        isOpen = true;
        keypadUI.SetActive(true);

        if (playerMovement != null)
            playerMovement.LockInput(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseKeypad()
    {
        if (!isOpen) return;

        isOpen = false;
        keypadUI.SetActive(false);

        if (playerMovement != null)
            playerMovement.LockInput(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ClearInput();
    }
}