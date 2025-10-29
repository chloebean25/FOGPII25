using UnityEngine;
using TMPro;

public class Notebook : MonoBehaviour
{
    public GameObject notebookUI;
    public TMP_InputField notes;

    public PlayerMovement playerMovement;

    public KeyCode toggleKey = KeyCode.N;
    private bool isOpen = false;

    public void Start()
    {
        notebookUI.SetActive(false);
        if (PlayerPrefs.HasKey("PlayerNotes")){
            notes.text = PlayerPrefs.GetString("PlayerNotes");
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Update()
    {
        if(Input.GetKeyDown(toggleKey)&& (notes == null || !notes.isFocused)){
            ToggleNotebook();
        }
    }
    public void ToggleNotebook()
    {
        isOpen = !isOpen;
        notebookUI.SetActive(isOpen);
        if (isOpen){
            //pause players movement
            if(playerMovement != null){
                playerMovement.LockInput(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                notes.Select();
            }
        }
        else{
            //resume players movement
            if(playerMovement != null){
                playerMovement.LockInput(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            SaveNotes();
        }
    }
    public void SaveNotes()
    {
        PlayerPrefs.SetString("PlayerNotes", notes.text);
        PlayerPrefs.Save();
    }
    
}
