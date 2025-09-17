using UnityEngine;
using TMPro;

public class KeyPadManager : MonoBehaviour
{
    public static KeyPadManager Instance;

    public GameObject keypadUI;
    public TMP_Text inputText;

    [Header("Settings")]
    public string correctCode = "8457";
    public Transform teleportSpot;
    public GameObject player;

    private string currentInput = "";
    public bool isOpen = false;

    private void Awake(){
        Instance =this;
        keypadUI.SetActive(false);
    }

    private void Update(){
        if(isOpen && Input.GetKeyDown(KeyCode.Escape)){
            CloseKeypad();
        }
    }

    public void AddDigit(string digit){
        if(currentInput.Length <10){
            currentInput += digit;
            inputText.text  = currentInput;
        }
    }

    public void ClearInput(){
        currentInput="";
        inputText.text ="";
    }

    public void SubmitCode(){
        if(currentInput== correctCode){
            player.transform.position = teleportSpot.position;
            CloseKeypad();
        }
        else{
            ClearInput();
        }
    }

    public void OpenKeypad(){
        isOpen = true;
        keypadUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log(">>> Keypad OPEN triggered");
        
    }
    public void CloseKeypad(){
        isOpen = false;
        keypadUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ClearInput();
    }
}

