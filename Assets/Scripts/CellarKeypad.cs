using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CellarKeypad : MonoBehaviour
{
    public static CellarKeypad Instance;

    [Header("References")]
    public GameObject keypadUI;
    public TMP_InputField inputField; 
    public PlayerMovement playerMovement;
    public string correctCode = "STAY LOW DON'T GO HATE THEM SACRIFICE";
    public string nextSceneName = "Farm2";

    public bool isOpen = false;

    private void Awake()
    {
        Instance = this;
        keypadUI.SetActive(false);

        
        inputField.lineType = TMP_InputField.LineType.SingleLine;

        
        inputField.onSubmit.AddListener(OnSubmit);
    }

    private void Update()
    {
        if (!isOpen) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
        }
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
            inputField.text = ""; 
            inputField.ActivateInputField(); 
        }
    }

    public void OpenKeypad()
    {
        if (isOpen) return;

        isOpen = true;
        keypadUI.SetActive(true);

        
        if (playerMovement != null)
            playerMovement.LockInput(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        
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
