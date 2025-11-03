using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Notebook : MonoBehaviour
{
    [Header("UI References")]
    public GameObject notebookUI;
    public TMP_InputField notes;

    [Header("Player Reference")]
    public PlayerMovement playerMovement;

    [Header("Controls")]
    public KeyCode toggleKey = KeyCode.N;
    public KeyCode closeKey = KeyCode.Escape;

    private bool isOpen = false;

    private void Start()
    {
        // Hide notebook at start
        notebookUI.SetActive(false);

        // Load saved notes if any
        if (PlayerPrefs.HasKey("PlayerNotes"))
            notes.text = PlayerPrefs.GetString("PlayerNotes");

        // Hide and lock cursor at start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Open notebook with toggleKey if not already open and not typing in another field
        if (Input.GetKeyDown(toggleKey) && !isOpen && !IsAnyInputFieldFocused())
        {
            ToggleNotebook();
        }

        // Close notebook with closeKey if open
        if (isOpen && Input.GetKeyDown(closeKey))
        {
            ToggleNotebook();
        }
    }

    // Returns true if any TMP_InputField is currently focused
    private bool IsAnyInputFieldFocused()
    {
        if (EventSystem.current == null || EventSystem.current.currentSelectedGameObject == null)
            return false;

        TMP_InputField input = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();
        return input != null && input.isFocused;
    }

    public void ToggleNotebook()
    {
        isOpen = !isOpen;
        notebookUI.SetActive(isOpen);

        if (isOpen)
        {
            // Lock player movement
            if (playerMovement != null)
                playerMovement.LockInput(true);

            // Show and unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Auto-focus the notebook input field
            notes.Select();
            notes.ActivateInputField();
        }
        else
        {
            // Unlock player movement
            if (playerMovement != null)
                playerMovement.LockInput(false);

            // Hide and lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Save notes
            SaveNotes();
        }
    }

    public void SaveNotes()
    {
        PlayerPrefs.SetString("PlayerNotes", notes.text);
        PlayerPrefs.Save();
    }
}
