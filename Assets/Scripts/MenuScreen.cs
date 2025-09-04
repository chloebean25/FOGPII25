using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    public void OnPlayButton()
    {
        SceneManager.LoadScene("Farm");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
