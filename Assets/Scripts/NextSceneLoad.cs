using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneLoad : MonoBehaviour
{
     private void LoadNextScene()
    {
        SceneManager.LoadScene("End");
    }
}

