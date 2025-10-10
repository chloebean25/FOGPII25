using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CellarKeypad : MonoBehaviour
{
    public static CellarKeypad Instance;

    [Header("References")]
    public GameObject keypadUI;
    public TMP_InputField inputField; // Use Input Field here
    public PlayerMovement playerMovement;
    public string correctCode = "STAY LOW DON'T GO HATE THEM SACRIFICE";
    public string nextSceneName = "Farm2";

    public bool isOpen = false;

    private void Awake()
    {
        Instance = this;
        keypadUI.SetActive(false);

        // Make sure Input Field is single line
        inputField.lineType = TMP_InputField.LineType.SingleLine;

        // Add submit listener
        inputField.onSubmit.AddListener(OnSubmit);
    }

    private void Update()
    {
        if (!isOpen) return;

        // Backspace is handled automatically by Input Field
        // Lock Escape or other keys if needed
    }

    private void OnSubmit(string text)
    {
        SubmitCode();
    }

    public void SubmitCode()
    {
        if (inputField.text.Equals(correctCode, System.StringComparison.OrdinalIgnoreCase))
        {
            SceneManager.LoadScene(nextSceneName);
            CloseKeypad();
        }
        else
        {
            inputField.text = ""; // Clear if wrong
            inputField.ActivateInputField(); // Keep focus
        }
    }

    public void OpenKeypad()
    {
        if (isOpen) return;

        isOpen = true;
        keypadUI.SetActive(true);

        // Lock player movement
        if (playerMovement != null)
            playerMovement.LockInput(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Activate input field and focus
        inputField.text = "";
        inputField.ActivateInputField();
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
    }
}
