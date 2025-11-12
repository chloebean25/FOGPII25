using UnityEngine;
using UnityEngine.SceneManagement;
public class AnimationController : MonoBehaviour
{
    public SceneController sceneController;

    
    public void CutToBlackBeforeEnd()
    {
        sceneController.CutToBlack();

        
        Invoke(nameof(LoadNextScene), 0.5f);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("Farm");
    }
}
