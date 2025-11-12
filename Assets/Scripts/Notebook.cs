using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Notebook : MonoBehaviour
{
    public GameObject notebookUI;
    public TMP_InputField notes;
    public PlayerMovement playerMovement;
    public KeyCode toggleKey = KeyCode.N;
    public KeyCode closeKey = KeyCode.Escape;

    private bool isOpen = false;

    private void Start()
    {
        PlayerPrefs.DeleteKey("PlayerNotes");
        notebookUI.SetActive(false);

        if (PlayerPrefs.HasKey("PlayerNotes"))
            notes.text = PlayerPrefs.GetString("PlayerNotes");

        notes.lineType = TMP_InputField.LineType.MultiLineNewline;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey) && !isOpen && !IsAnyInputFieldFocused())
        {
            ToggleNotebook();
        }

        if (isOpen && Input.GetKeyDown(closeKey))
        {
            ToggleNotebook();
        }
    }

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
            if (playerMovement != null)
                playerMovement.LockInput(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            notes.ActivateInputField();
            notes.caretPosition = notes.text.Length;
            notes.selectionAnchorPosition = notes.caretPosition;
            notes.selectionFocusPosition = notes.caretPosition;
        }
        else
        {
            if (playerMovement != null)
                playerMovement.LockInput(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SaveNotes();
        }
    }

    public void SaveNotes()
    {
        PlayerPrefs.SetString("PlayerNotes", notes.text);
        PlayerPrefs.Save();
    }
}
