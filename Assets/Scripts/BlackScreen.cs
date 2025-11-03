using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreen : MonoBehaviour
{
    [Header("UI")]
    public GameObject blackScreen;

    [Header("Scene Settings")]
    public string nextSceneName = "Farm";

    [Header("Truck Reference")]
    public Animator truckAnimator;
    public string truckMoveStateName = "TruckMove"; 

    
    public void CutToBlack()
    {
        blackScreen.SetActive(true);
        StartCoroutine(WaitForTruckAnimation());
    }

    private System.Collections.IEnumerator WaitForTruckAnimation()
    {
        if (truckAnimator == null)
        {
            Debug.LogWarning("Truck Animator not assigned! Loading scene immediately.");
            SceneManager.LoadScene(nextSceneName);
            yield break;
        }

        
        AnimationClip clip = null;
        foreach (var c in truckAnimator.runtimeAnimatorController.animationClips)
        {
            if (c.name == truckMoveStateName)
            {
                clip = c;
                break;
            }
        }

        if (clip == null)
        {
            Debug.LogWarning($"Truck animation clip '{truckMoveStateName}' not found! Loading scene immediately.");
            SceneManager.LoadScene(nextSceneName);
            yield break;
        }

        
        yield return new WaitForSeconds(clip.length);

        blackScreen.SetActive(false);
        SceneManager.LoadScene(nextSceneName);
    }
}
