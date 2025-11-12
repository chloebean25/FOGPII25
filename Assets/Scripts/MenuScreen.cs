using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuScreen : MonoBehaviour
{
    public GameObject controlsText;
    public SceneController sceneController;

    public void Start(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        controlsText.SetActive(false);
    }
    public void OnPlayButton()
    {
        sceneController.LoadScene("IntroAnimation");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnControlsButton()
    {
        controlsText.SetActive(true);
    }
    public void OnExitButton()
    {
        controlsText.SetActive(false);
    }
}
 